using ApiProj.DTOs;
using ApiProj.Models;

namespace ApiProj.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts(PaginationFilter filter);
        Task<Product> UpdateProductAsync(int id, ProductUpdateDTO Product);
        Product GetProductById(int id);
        void AddProduct(Product product);
    }
}
