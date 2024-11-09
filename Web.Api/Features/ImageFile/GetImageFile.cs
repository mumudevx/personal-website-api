namespace Web.Api.Features.ImageFile;

public class GetImageFile : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        string Name,
        string Alt,
        string Path,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private class GetImageFileValidator : AbstractValidator<Request>
    {
        public GetImageFileValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetImageFileMapperProfile : Profile
    {
        public GetImageFileMapperProfile()
        {
            CreateMap<Request, Entities.ImageFile>();
            CreateMap<Entities.ImageFile, Response>();
        }
    }

    internal sealed class Handler(
        IImageFileRepository imageFileRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly GetImageFileValidator _validator = new(validationMessages);

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

            var entity = await imageFileRepository.GetAsync(
                imageFile => imageFile.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/imageFile/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Image File");
    }
}