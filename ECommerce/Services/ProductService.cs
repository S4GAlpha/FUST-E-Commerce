using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Components.Forms;

namespace ECommerce.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task SaveProductAsync(Product product, IBrowserFile? selectedFile)
        {
            if (selectedFile != null)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", selectedFile.Name);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await selectedFile.OpenReadStream().CopyToAsync(stream);
                }
                product.ImageUrl = $"/images/{selectedFile.Name}";
            }

            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.CategoryId = product.CategoryId;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
