using ApiProj.DTOs;
using ApiProj.Interfaces;
using ApiProj.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiProj.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductService _productService;

    public ProductsController(IProductService productService,IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> Get()
    {
        var products = _productRepository.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDTO> Get(int id)
    {
        var product = _productRepository.GetProductById(id);

        if (product == null)
        {
            return NotFound(new {Message = $"Product with id {id} not found"});
        }
        
        return Ok(product);
    }


    [HttpPost]
    public ActionResult AddProduct([FromBody] ProductCreateDTO product)
    {
        var createdProduct = _productService.AddProduct(product);

        if(createdProduct is null)
            return BadRequest(new { Message = "Could not create product" });

        return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct); //best practice to return the created resource
    }

    [HttpPut("{id}")]
    public ActionResult UpdateProduct(int id, [FromBody] ProductUpdateDTO product)
    {
        var productToUpdate = _productRepository.GetProductById(id);

        if (productToUpdate is null)
            return NotFound(new { Message = "Product not found" });

        var category = _categoryRepository.GetCategoryById(product.CategoryId);
        if (category is null)
            return BadRequest(new { Message = $"Category with id {product.CategoryId} not found" });


        //can be deduced by using auto-mapper but for simplicity we do it manually here
        productToUpdate.Id = product.Id;
        productToUpdate.Name = product.Name;
        productToUpdate.CategoryId = product.CategoryId;
        productToUpdate.Price = product.Price;

        return NoContent();
    }
}