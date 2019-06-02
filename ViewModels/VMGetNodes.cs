using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class VMGetNodes
    {
        public Guid IdNode { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
