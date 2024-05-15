using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerStoreApi.Models;

namespace ComputerStoreApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id, bool includeCategory = false);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);

        Task UpdateProductStockAsync(string productName, int quantity);
        Task<Product> GetProductByNameAsync(string productName);
    }
}


