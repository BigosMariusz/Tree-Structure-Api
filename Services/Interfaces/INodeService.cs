using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Services.Interfaces
{
    public interface INodeService
    {
        Task<List<VMGetNodes>> GetNodesAsync();
        Task<VMGetNodes> CreateAsync(VMAddNodes node);
        Task<List<VMDeleteNodeResult>> DeleteAsync(Guid id);
        Task<VMGetNodes> UpdateAsync(VMEditNode node, Guid id);
        Task MoveNode(VMMoveNodeData node, Guid id);
    }
}
