using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
