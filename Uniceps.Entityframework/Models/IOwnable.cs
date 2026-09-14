using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public interface IOwnable
    {
        string? UserId { get; set; }
    }
}
