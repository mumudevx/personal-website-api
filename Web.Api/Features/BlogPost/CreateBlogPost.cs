namespace Web.Api.Features.BlogPost;

public class CreateBlogPost : IFeature
{
    public record Request(
        string Title,
        string ShortContent,
        string Slug,
        string Content,
        Guid FeaturedImageId,
        List<string> Tags
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreateBlogPostValidator : AbstractValidator<Request>
    {
        public CreateBlogPostValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage(validationMessages.TitleRequired);

            RuleFor(request => request.ShortContent)
                .NotEmpty().WithMessage(validationMessages.ShortContentRequired)
                .MaximumLength(300).WithMessage(validationMessages.ShortContentMaxLength);

            RuleFor(request => request.Slug)
                .NotEmpty().WithMessage(validationMessages.SlugRequired);

            RuleFor(request => request.Content)
                .NotEmpty().WithMessage(validationMessages.ContentRequired);

            RuleFor(request => request.FeaturedImageId)
                .NotEmpty().WithMessage(validationMessages.FeaturedImageRequired);
        }
    }

    public class CreateBlogPostMapperProfile : Profile
    {
        public CreateBlogPostMapperProfile()
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
        private readonly CreateBlogPostValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.BlogPost>(request);

            await blogPostRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/blogPost", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Blog Post");
    }
}