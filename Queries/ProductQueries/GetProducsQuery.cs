using ApiProj.Wrappers;
using MediatR;
using ApiProj.Models;

namespace ApiProj.Queries.ProductQueries
{
    public class GetProducsQuery : IRequest<PagedResponse<List<Product>>>
    {

    }
}
