using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public interface IProductRelatedDataService<T>
    {
        Task<T> Create(T entity);
        Task<bool> Delete(int id);
        Task<T> Update(T entity);
        Task<IEnumerable<T>> GetAllByProductId(int productId);
    }
}
