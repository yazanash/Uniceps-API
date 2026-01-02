using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public interface IProductDataService
    {
        Task<Product> Create(Product entity);
        Task<bool> Delete(int id);
        Task<Product> Get(int id);
        Task<Product> GetByAppId(int appId);
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Update(Product entity);

    }
}
