namespace Web.Api.Features;

public interface IFeature
{
    RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app);
}