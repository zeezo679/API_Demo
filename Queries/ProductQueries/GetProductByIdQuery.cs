using ApiProj.Models;
using MediatR;

namespace ApiProj.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}
