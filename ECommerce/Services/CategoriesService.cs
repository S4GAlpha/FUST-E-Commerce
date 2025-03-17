using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

public class CategoriesService
{
    private readonly ApplicationDbContext _dbContext;

    public CategoriesService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task SaveCategoryAsync(Category category)
    {
        if (category.Id == 0)
        {
            _dbContext.Categories.Add(category);
        }
        else
        {
            var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
