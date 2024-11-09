namespace Web.Api.Features.BlogPost;

public class GetBlogPost : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        string Title,
        string ShortContent,
        string Slug,
        string Content,
        Guid FeaturedImageId,
        List<string> Tags,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    private class GetBlogPostValidator : AbstractValidator<Request>
    {
        public GetBlogPostValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetBlogPostMapperProfile : Profile
    {
        public GetBlogPostMapperProfile()
        {
            CreateMap<Request, Entities.BlogPost>();
            CreateMap<Entities.BlogPost, Response>();
        }
    }

    internal sealed class Handler(
        IBlogPostRepository blogPostRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly GetBlogPostValidator _validator = new(validationMessages);

        public async Task<IDataResult<Response>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationMessages = validationResult.ToString("<br>");
                return new ErrorDataResult<Response>(message: validationMessages);
            }

            var entity = await blogPostRepository.GetAsync(
                blogPost => blogPost.Id == request.Id,
                ["FeaturedImage"],
                cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/blogPost/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Blog Post");
    }
}