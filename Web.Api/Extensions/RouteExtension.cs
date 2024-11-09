namespace Web.Api.Extensions;

public static class RouteExtension
{
    public static void AddRoutes(this IEndpointRouteBuilder builder)
    {
        var modules = typeof(IFeature).Assembly.GetTypes()
            .Where(p => p is { IsClass: true, IsAbstract: false } && typeof(IFeature).IsAssignableFrom(p))
            .Select(Activator.CreateInstance)
            .Cast<IFeature>()
            .ToList();

        foreach (var module in modules)
            module.AddRoutes(builder);
    }
}