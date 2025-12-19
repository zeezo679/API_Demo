using ApiProj.DTOs;
using ApiProj.Interfaces;
using ApiProj.Models;
using ApiProj.Wrappers;
using Asp.Versioning;
using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace ApiProj.Controllers;

[ApiVersion("1.0")]
[ApiVersion("3.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductService _productService;
    private LinkGenerator _linkGenerator;

    public ProductsController(IProductService productService,IProductRepository productRepository, ICategoryRepository categoryRepository, LinkGenerator linkGenerator)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productService = productService;
        _linkGenerator = linkGenerator;
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<Product>), 200)]
    public ActionResult<IEnumerable<ProductDTO>> Get([FromServices] IProductService productService, [FromQuery] PaginationFilter filter)
    {
        var paginationFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var products = productService.GetAllProducts(paginationFilter);

        return Ok(new PagedResponse<List<Product>>(products, filter.PageNumber, filter.PageSize, new Uri($"https://localhost:7014/api/v1/products?PageNumber={filter.PageNumber+1}&PageSize={filter.PageSize=10}")));
    }

    [MapToApiVersion("3.0")]
    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> GetV3([FromServices] IProductService productService, [FromQuery] PaginationFilter filter)
    {
        var paginationFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var products = productService.GetAllProducts(paginationFilter);
        return Ok("This version of api doesnt return body");
    }

    [MapToApiVersion("1.0")]
    [MapToApiVersion("3.0")]
    [HttpGet("{id}",Name = "GetProduct")]
    [ProducesResponseType(typeof(Response<Product>), 200)]
    public ActionResult<ProductDTO> Get(int id)
    {
        
        var product = _productRepository.GetProductById(id);

        if (product == null)
        {
            return NotFound(new {Message = $"Product with id {id} not found"});
        }

        return StatusCode(200, new Response<Product>(product));
    }


    [MapToApiVersion("1.0")]
    [MapToApiVersion("3.0")]
    [HttpPost]
    public ActionResult AddProduct([FromBody] ProductCreateDTO product)
    {
        var createdProduct = _productService.AddProduct(product);

        if (createdProduct is null)
            return BadRequest(new { Message = "Could not create product" });

        return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct); //best practice to return the created resource
    }


    [MapToApiVersion("1.0")]
    [MapToApiVersion("3.0")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO product)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Error in Model State");
        }

        var result = await _productService.UpdateProduct(id, product);

        if (result is null)
            return NotFound("Product is not found, updated unsuccessfull");

        return Ok("Product Updated");
    }
}

//