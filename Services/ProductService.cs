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

        public List<Product> GetAllProducts(PaginationFilter filter)
        {
            var products = _productRepository.GetAllProducts(filter);
            return products;
        }

        public ProductDTO? GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> UpdateProduct(int id, ProductUpdateDTO product)
        {
            var updatedProduct = await _productRepository.UpdateProductAsync(id, product);

            if (updatedProduct is null)
                return null;

            return updatedProduct;
        }
    }
}
