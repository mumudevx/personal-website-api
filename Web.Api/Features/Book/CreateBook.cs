namespace Web.Api.Features.Book;

public class CreateBook : IFeature
{
    public record Request(
        string Title,
        string Author,
        string? Publisher,
        string? Isbn,
        int? Year,
        int Pages,
        Guid FeaturedImageId,
        DateTime StartReadingDate,
        List<string> Tags
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreateBookValidator : AbstractValidator<Request>
    {
        public CreateBookValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage(validationMessages.TitleRequired);

            RuleFor(request => request.Author)
                .NotEmpty().WithMessage(validationMessages.AuthorRequired);

            RuleFor(request => request.Pages)
                .NotEmpty().WithMessage(validationMessages.PagesRequired);

            RuleFor(request => request.FeaturedImageId)
                .NotEmpty().WithMessage(validationMessages.FeaturedImageRequired);

            RuleFor(request => request.StartReadingDate)
                .NotEmpty().WithMessage(validationMessages.StartReadingDateRequired);
        }
    }

    public class CreateBookMapperProfile : Profile
    {
        public CreateBookMapperProfile()
        {
            CreateMap<Request, Entities.Book>();
            CreateMap<Entities.Book, Response>();
        }
    }

    internal sealed class Handler(
        IBookRepository bookRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly CreateBookValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.Book>(request);

            await bookRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/book", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Book");
    }
}