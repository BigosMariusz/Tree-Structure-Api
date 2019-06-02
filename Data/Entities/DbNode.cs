using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class DbNode
    {
        [Key]
        public Guid IdNode { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
