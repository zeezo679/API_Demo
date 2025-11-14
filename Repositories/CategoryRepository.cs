using ApiProj.Infrastructure;
using ApiProj.Interfaces;
using ApiProj.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProj.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public List<Category> GetAllCategories()
    {
        var categories = _context.Categories.AsNoTracking().ToList();
        return categories;
    }

    public Category? GetCategoryById(int id)
    {
        var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
        return category;
    }
}
