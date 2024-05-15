using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStoreApi.Models;
using ComputerStoreApi.Repositories;

namespace ComputerStoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id, bool includeCategory = false)
        {
            var query = _context.Products.AsQueryable();

            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductByNameAsync(string productName) // Add this method
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product with the given ID not found.");
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product with the given ID not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateProductStockAsync(string productName, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            if (product != null)
            {
                product.Quantity = quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}

