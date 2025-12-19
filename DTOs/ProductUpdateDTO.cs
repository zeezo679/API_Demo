using ApiProj.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiProj.DTOs;

public class ProductUpdateDTO
{
    [Required(ErrorMessage = "Product Id is required to update the product")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Must enter the price")]
    [Range(0, 1000, ErrorMessage = "Must be between 0 and 1000")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Must enter associated category id")]
    public int CategoryId { get; set; }
}