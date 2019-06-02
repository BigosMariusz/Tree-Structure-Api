using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Initialize
{
    public class InitializeNodes
    {
        public static async Task InitializeData(TreeContext context)
        {
            context.Database.Migrate();

            if (!context.Nodes.Any())
            {
                var node = new DbNode[]
                {
                    new DbNode
                    {
                        Level = 0,
                        Name = "root",
                        ParentId = null
                    }
                };
                context.Nodes.AddRange(node);
                await context.SaveChangesAsync();

                var rootId = context.Nodes.FirstOrDefault().IdNode;

                var nodes = new DbNode[] {
                    new DbNode
                    {
                        Level = 1,
                        Name = "First",
                        ParentId = rootId
                    },
                    new DbNode
                    {
                        Level = 1,
                        Name = "Second",
                        ParentId = rootId
                    },
                    new DbNode
                    {
                        Level = 1,
                        Name = "Text",
                        ParentId = rootId
                    },
                    new DbNode
                    {
                        Level = 1,
                        Name = "Any",
                        ParentId = rootId
                    }
                };
                context.Nodes.AddRange(nodes);
                await context.SaveChangesAsync();
            }
        }
    }
}
