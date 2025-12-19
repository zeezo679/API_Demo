using ApiProj.DTOs;
using ApiProj.Infrastructure;
using ApiProj.Interfaces;
using ApiProj.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProj.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            //happy path :)
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<Product> GetAllProducts(PaginationFilter filter)
        {
            var products = _context.Products
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToList();

            return products;
        }

        public Product? GetProductById(int id)
        {
            var product = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Category = p.Category
                })
                .FirstOrDefault(p => p.Id == id);

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductUpdateDTO product)
        {

            if(product is null)
            {
                return null;
            }

            var productToUpdate = _context.Products.FirstOrDefault(p => p.Id == id);

            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.CategoryId = product.CategoryId;
            
            _context.SaveChanges();
            return productToUpdate;
        }
    }
}
