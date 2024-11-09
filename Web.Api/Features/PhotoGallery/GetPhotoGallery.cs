namespace Web.Api.Features.PhotoGallery;

public class GetPhotoGallery : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        string Title,
        string Slug,
        string? Description,
        //List<PhotoItem> Photos,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private class GetPhotoGalleryValidator : AbstractValidator<Request>
    {
        public GetPhotoGalleryValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetPhotoGalleryMapperProfile : Profile
    {
        public GetPhotoGalleryMapperProfile()
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
        private readonly GetPhotoGalleryValidator _validator = new(validationMessages);

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

            var entity = await photoGalleryRepository.GetAsync(
                photoGallery => photoGallery.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/photoGallery/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Photo Gallery");
    }
}