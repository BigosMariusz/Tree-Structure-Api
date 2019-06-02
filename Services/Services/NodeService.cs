using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Services.Services
{
    public class NodeService : INodeService
    {
        private readonly TreeContext _context;
        private readonly DataValidator _validator = new DataValidator();
        public NodeService(TreeContext context)
        {
            _context = context;
        }
        public async Task<List<VMGetNodes>> GetNodesAsync()
        {
            var dbNodes = await _context.Nodes.Where(x => x.IdNode != Guid.Empty).ToListAsync();

            List<VMGetNodes> nodesList = new List<VMGetNodes>();
            foreach (var node in dbNodes)
            {
                var newNode = new VMGetNodes()
                {
                    IdNode = node.IdNode,
                    Level = node.Level,
                    Name = node.Name,
                    ParentId = node.ParentId
                };
                nodesList.Add(newNode);
            }
            return nodesList;
        }
        public async Task<VMGetNodes> CreateAsync(VMAddNodes node)
        {
            if (!_validator.Validate(node.Name))
                throw new ArgumentException("Data not valid. Name must has maximum 30 chars.");

            var level =  _context.Nodes.Where(x => x.IdNode == node.ParentId).Select(x => x.Level).FirstOrDefault();
            var newNode = new DbNode
            {
                Level = level + 1,
                Name = node.Name,
                ParentId = node.ParentId
            };

            var result = await _context.Nodes.AddAsync(newNode);
            _context.SaveChanges();

            var dbNode = result.Entity;
            var viewModel = new VMGetNodes();
            viewModel.IdNode = dbNode.IdNode;
            viewModel.Level = dbNode.Level;
            viewModel.Name = dbNode.Name;
            viewModel.ParentId = dbNode.ParentId;

            return viewModel;
        }

        public async Task<List<VMDeleteNodeResult>> DeleteAsync(Guid id)
        {
            List<DbNode> deletedNodes = new List<DbNode>();
            var node = new DbNode
            {
                IdNode = id
            };
            try
            {
                var result = _context.Nodes.Remove(node);
                deletedNodes.Add(result.Entity);
                var childs = await _context.Nodes.Where(x => x.ParentId == result.Entity.IdNode).Select(x => x.IdNode).ToListAsync();
                foreach (var childId in childs)
                {
                    node = new DbNode
                    {
                        IdNode = childId
                    };
                    result = _context.Nodes.Remove(node);
                    deletedNodes.Add(result.Entity);
                }
            }
            catch (ArgumentException)
            {
                return null;
            }

            await _context.SaveChangesAsync();

            var viewModels = new List<VMDeleteNodeResult>();
            foreach (var deletedNode in deletedNodes)
            {
                var viewModel = new VMDeleteNodeResult();
                viewModel.IdNode = deletedNode.IdNode;
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public async Task<VMGetNodes> UpdateAsync(VMEditNode node, Guid id)
        {
            if (!_validator.Validate(node.Name))
                throw new ArgumentException("Data not valid. Name must start with character and has maximum 30 chars.");

            var nodeToEdit = _context.Nodes.Where(x => x.IdNode == id).FirstOrDefault();
            if (nodeToEdit == null)
            {
                return null;
            }

            nodeToEdit.Name = node.Name;
            var result = _context.Nodes.Update(nodeToEdit);
            await _context.SaveChangesAsync();
            var editedNode = result.Entity;

            var viewModel = new VMGetNodes();
            viewModel.IdNode = editedNode.IdNode;
            viewModel.Level = editedNode.Level;
            viewModel.Name = editedNode.Name;
            viewModel.ParentId = editedNode.ParentId;

            return viewModel;
        }

        public async Task MoveNode(VMMoveNodeData node, Guid id)
        {
            var nodeList = new List<DbNode>();
            var destinationLevel = _context.Nodes.Where(x => x.IdNode == node.DestinationId).Select(x => x.Level).FirstOrDefault();
            var slectedName = _context.Nodes.Where(x => x.IdNode == id).Select(x => x.Name).FirstOrDefault();
            var newNode = new DbNode
            {
                IdNode = id,
                Name = slectedName,
                Level = destinationLevel + 1,
                ParentId = node.DestinationId
            };
            nodeList.Add(newNode);
            var childsList = _context.Nodes.Where(x => x.ParentId == id).ToList();
            foreach (var child in childsList)
            {
                newNode = new DbNode
                {
                    IdNode = child.IdNode,
                    Name = child.Name,
                    Level = destinationLevel + 2,
                    ParentId = child.ParentId
                };
                nodeList.Add(newNode);
            }

            await DeleteAsync(id);

            foreach (var createdNode in nodeList)
            {
                var result = await _context.Nodes.AddAsync(createdNode);
            }
            _context.SaveChanges();
        }
    }
}
