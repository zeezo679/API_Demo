using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IEnumerable<EndpointDataSource> _endpointDataSources;
        public DiagnosticsController(IEnumerable<EndpointDataSource> endpointDataSources)
        {
            _endpointDataSources = endpointDataSources;
        }
        // Returns detailed runtime information about all discovered endpoints,
        // including route templates, parameters, constraints, defaults, HTTP methods, and metadata.
        [HttpGet("endpoints")]
        public ActionResult<IEnumerable<EndpointDetailsDTO>> GetAllEndpointsDetailed()
        {
            var results = new List<EndpointDetailsDTO>();
            // Iterate through each endpoint source registered in DI.
            foreach (var dataSource in _endpointDataSources)
            {
                // Each data source may represent controllers, minimal APIs, Swagger, etc.
                foreach (var endpoint in dataSource.Endpoints)
                {
                    // Extract Common Metadata
                    // RouteNameMetadata and EndpointNameMetadata store developer-assigned names.
                    var routeName = endpoint.Metadata.GetMetadata<IRouteNameMetadata>()?.RouteName;
                    var endpointName = endpoint.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
                    // HttpMethodMetadata contains information about allowed HTTP verbs (GET, POST, etc.)
                    var httpMethodMetadata = endpoint.Metadata.GetMetadata<HttpMethodMetadata>();
                    var httpMethods = httpMethodMetadata?.HttpMethods?.ToArray() ?? Array.Empty<string>();
                    // Attribute metadata: collects attribute types applied to controller actions (e.g., [HttpGet], [Authorize]).
                    var attributeTypeNames = endpoint.Metadata
                        .OfType<Attribute>()
                        .Select(a => a.GetType().FullName ?? a.GetType().Name)
                        .ToArray();
                    // Initialize DTO with base details
                    var details = new EndpointDetailsDTO
                    {
                        DisplayName = endpoint.DisplayName, // human-readable identifier
                        EndpointType = endpoint.GetType().FullName ?? endpoint.GetType().Name, // RouteEndpoint, Endpoint, etc.
                        IsRouteEndpoint = endpoint is RouteEndpoint, // true if it supports a route pattern
                        RouteName = routeName,
                        EndpointName = endpointName,
                        HttpMethods = httpMethods,
                        AttributeMetadataTypeNames = attributeTypeNames
                    };
                    // Extract Route-Specific Data (if this is a RouteEndpoint)
                    if (endpoint is RouteEndpoint routeEndpoint)
                    {
                        var pattern = routeEndpoint.RoutePattern;
                        // Raw route pattern text (e.g., "api/products/{id:int:min(1)=1}")
                        details.Pattern = pattern.RawText;
                        // Route Defaults
                        // Example: {id=1} produces "id" -> "1"
                        if (pattern.Defaults.Count > 0)
                        {
                            details.PatternDefaults = pattern.Defaults
                                .ToDictionary(kv => kv.Key, kv => kv.Value?.ToString());
                        }
                        // Route Parameters
                        // Each parameter may have:
                        //   - a name (e.g., "id")
                        //   - an optional flag (e.g., "{id?}")
                        //   - a default value (e.g., "{id=1}")
                        //   - constraints/policies (e.g., ":int", ":min(1)")
                        if (pattern.Parameters.Count > 0)
                        {
                            details.Parameters = pattern.Parameters
                                .Select(p => new RouteParameterDetailsDTO
                                {
                                    Name = p.Name,
                                    IsOptional = p.IsOptional,
                                    DefaultValue = p.Default?.ToString(),
                                    // Each ParameterPolicyReference contains a "Content" object that describes the constraint.
                                    // Converting it to string gives readable names like "int", "min(1)", etc.
                                    Policies = p.ParameterPolicies
                                        .Select(pol => pol.Content?.ToString() ?? string.Empty)
                                        .ToArray()
                                })
                                .ToArray();
                        }
                    }
                    // Add to results
                    results.Add(details);
                }
            }
            // Return as JSON
            return Ok(results);
        }
    }

    public sealed class EndpointDetailsDTO
    {
        // ---------- General ----------
        public string? DisplayName { get; set; }             // e.g., "ProductsController.GetById (RoutingInASPNETCoreWebAPI)"
        public string? EndpointType { get; set; }            // e.g., "Microsoft.AspNetCore.Routing.RouteEndpoint"
        public bool IsRouteEndpoint { get; set; }            // true if it's a RouteEndpoint (as opposed to a non-routed endpoint like Swagger)
        // ---------- Route Endpoint Specific ----------
        public string? Pattern { get; set; }                 // Full route pattern string (e.g., "api/products/{id:int:min(1)=1}")
        public Dictionary<string, string?>? PatternDefaults { get; set; } // Route-level or parameter-level default values
        public RouteParameterDetailsDTO[]? Parameters { get; set; }       // All parameters in the pattern
        // ---------- Metadata ----------
        public string? RouteName { get; set; }               // From [HttpGet(Name = "...")] or similar
        public string? EndpointName { get; set; }            // From EndpointNameMetadata (used by minimal APIs)
        public string[] HttpMethods { get; set; } = Array.Empty<string>(); // Allowed HTTP verbs (GET, POST, PUT, etc.)
        public string[] AttributeMetadataTypeNames { get; set; } = Array.Empty<string>(); // Attribute metadata attached to this endpoint
    }
    // Represents details about a single route parameter, including constraints and defaults.
    public sealed class RouteParameterDetailsDTO
    {
        public string Name { get; set; } = string.Empty;     // Parameter name (e.g., "id")
        public bool IsOptional { get; set; }                 // True if the parameter is optional ({id?})
        public string? DefaultValue { get; set; }            // Default value if defined ({id=1})
        public string[] Policies { get; set; } = Array.Empty<string>(); // Parameter constraints/policies (e.g., "int", "min(1)")
    }
}
