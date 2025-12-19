using ApiProj.Models;

namespace ApiProj.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public int CategoryId { get; set; }
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    public Category Category { get; set; }

}