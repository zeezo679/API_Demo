using ApiProj.DTOs;
using ApiProj.Interfaces;
using ApiProj.Models;
using ApiProj.Repositories;

namespace ApiProj.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public ProductCreateDTO? AddProduct(ProductCreateDTO product)
        {
            if (product is null)
                return null;

            var category = _categoryRepository.GetCategoryById(product.CategoryId);
            if (category is null)
                return null;

            var productToAdd = new Product
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = category.Id,
            };

            _productRepository.AddProduct(productToAdd);

            return new ProductCreateDTO
            {
                Id = productToAdd.Id,
                Name = productToAdd.Name,
                Price = productToAdd.Price,
                CategoryId = productToAdd.CategoryId
            };
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public ProductDTO? GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public ProductUpdateDTO? UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
