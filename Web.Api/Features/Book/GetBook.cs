namespace Web.Api.Features.Book;

public class GetBook : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        string Title,
        string Author,
        string? Publisher,
        string? Isbn,
        int? Year,
        int Pages,
        string? Comment,
        float? Rating,
        Guid FeaturedImageId,
        DateTime StartReadingDate,
        DateTime? EndReadingDate,
        int? DaysToComplete,
        List<string> Tags,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    private class GetBookValidator : AbstractValidator<Request>
    {
        public GetBookValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetBookMapperProfile : Profile
    {
        public GetBookMapperProfile()
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
        private readonly GetBookValidator _validator = new(validationMessages);

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

            var entity = await bookRepository.GetAsync(
                book => book.Id == request.Id,
                ["FeaturedImage"],
                cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/book/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Book");
    }
}