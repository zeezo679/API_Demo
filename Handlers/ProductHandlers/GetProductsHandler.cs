using ApiProj.Models;
using ApiProj.Queries.ProductQueries;
using ApiProj.Wrappers;
using MediatR;

namespace ApiProj.Handlers.ProductHandlers
{
    public class GetProductsHandler : IRequestHandler<GetProducsQuery, PagedResponse<List<Product>>>
    {
        public Task<PagedResponse<List<Product>>> Handle(GetProducsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
