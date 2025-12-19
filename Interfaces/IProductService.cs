using ApiProj.DTOs;
using ApiProj.Models;

namespace ApiProj.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts(PaginationFilter filter);
        ProductDTO? GetProductById(int id);
        ProductCreateDTO? AddProduct(ProductCreateDTO product);
        Task<Product> UpdateProduct(int id, ProductUpdateDTO productToUpdate);
    }
}
