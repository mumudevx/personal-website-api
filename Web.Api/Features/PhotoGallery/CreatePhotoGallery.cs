namespace Web.Api.Features.PhotoGallery;

public class CreatePhotoGallery : IFeature
{
    public record Request(
        string Title,
        string Slug,
        string? Description,
        List<Guid>? PhotoIds
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreatePhotoGalleryValidator : AbstractValidator<Request>
    {
        public CreatePhotoGalleryValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage(validationMessages.TitleRequired)
                .MaximumLength(150).WithMessage(validationMessages.TitleMaxLength);

            RuleFor(request => request.Slug)
                .NotEmpty().WithMessage(validationMessages.SlugRequired);

            RuleFor(request => request.Description)
                .MaximumLength(500).WithMessage(validationMessages.DescriptionMaxLength);
        }
    }

    public class CreatePhotoGalleryMapperProfile : Profile
    {
        public CreatePhotoGalleryMapperProfile()
        {
            CreateMap<Request, Entities.PhotoGallery>();
            CreateMap<Entities.PhotoGallery, Response>();
        }
    }

    internal sealed class Handler(
        IPhotoGalleryRepository photoGalleryRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly CreatePhotoGalleryValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.PhotoGallery>(request);

            await photoGalleryRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/photoGallery", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Photo Gallery");
    }
}