using ApiProj.Models;

namespace ApiProj.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);

        void AddProduct(Product product);
    }
}
