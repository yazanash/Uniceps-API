using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public abstract class EntityBase
    {
        [Key]
        public Guid NID { get; set; }
    }
}
