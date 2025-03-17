using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ECommerce.Services
{
    public class ImportService
    {
        private readonly ApplicationDbContext _context;

        public ImportService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task ImportCategoriesFromCsv(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            foreach (var line in lines.Skip(1)) // Salta l'intestazione
            {
                var values = line.Split(',');
                var name = values[0].Trim();
                var description = values[1].Trim();

                // Controlla se la categoria esiste già
                var existingCategory = _context.Categories.FirstOrDefault(c => c.Name == name);
                if (existingCategory == null)
                {
                    var newCategory = new Category
                    {
                        Name = name,
                        Description = description
                    };
                    _context.Categories.Add(newCategory);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task ImportProductsFromCsv(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            foreach (var line in lines.Skip(1)) // Salta l'intestazione
            {
                var values = line.Split(',');
                var name = values[0].Trim();
                var description = values[1].Trim();
                var price = decimal.Parse(values[2].Trim(), CultureInfo.InvariantCulture);
                var categoryId = int.Parse(values[3].Trim());
                var imageUrl = values[4].Trim();

                // Controlla se il prodotto esiste già
                var existingProduct = _context.Products.FirstOrDefault(p => p.Name == name);
                if (existingProduct == null)
                {
                    // Controlla se la categoria esiste
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category == null)
                    {
                        throw new Exception($"Categoria con ID {categoryId} non trovata.");
                    }

                    var newProduct = new Product
                    {
                        Name = name,
                        Description = description,
                        Price = price,
                        CategoryId = categoryId,
                        ImageUrl = imageUrl,
                        CreationDate = DateTime.UtcNow
                    };
                    _context.Products.Add(newProduct);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
