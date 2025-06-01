using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IMapperExtension<TDomain, TRead, TCreate>
    {
        public TRead ToDto(TDomain data);
        public TDomain FromCreationDto(TCreate data);
    }
}
