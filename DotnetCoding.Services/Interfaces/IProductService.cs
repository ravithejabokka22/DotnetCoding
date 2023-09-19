using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces
{
    public interface IProductService
    {
        ProductDetails GetProduct(int Id);
        Task<IEnumerable<ProductDetails>> GetAllProducts();
        Task CreateProduct(ProductDetails product);
        void UpdateProduct(ProductDetails product);
        void DeleteProduct(int Id);

        Task<IEnumerable<ProductDetails>> SearchProducts(Dictionary<string,string> searchItems);
    }
}
