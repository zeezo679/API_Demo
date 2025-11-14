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

        public List<Product> GetAllProducts()
        {
            var products = _context.Products.AsNoTracking().ToList();
            return products;
        }

        public Product? GetProductById(int id)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            return product;
        }
    }
}
