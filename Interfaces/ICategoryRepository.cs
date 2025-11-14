using ApiProj.Models;

namespace ApiProj.Interfaces;

public interface ICategoryRepository
{
    List<Category> GetAllCategories();
    Category? GetCategoryById(int id);
    void AddCategory(Category category);
}
