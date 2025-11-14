using ApiProj.DTOs;
using ApiProj.Models;

namespace ApiProj.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        ProductDTO? GetProductById(int id);
        ProductCreateDTO? AddProduct(ProductCreateDTO product);
        ProductUpdateDTO? UpdateProduct(Product product);
    }
}
