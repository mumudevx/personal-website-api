namespace Web.Api.Features.PhotoItem;

public class CreatePhotoItem : IFeature
{
    public record Request(
        string Title,
        string? Description,
        int Order,
        Guid PhotoGalleryId,
        Guid ImageFileId,
        List<string> Tags
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreatePhotoItemValidator : AbstractValidator<Request>
    {
        public CreatePhotoItemValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage(validationMessages.TitleRequired)
                .MaximumLength(150).WithMessage(validationMessages.TitleMaxLength);

            RuleFor(request => request.Description)
                .MaximumLength(500).WithMessage(validationMessages.DescriptionMaxLength);

            RuleFor(request => request.Order)
                .NotEmpty().WithMessage(validationMessages.OrderRequired)
                .GreaterThanOrEqualTo(0).WithMessage(validationMessages.OrderMinValue);

            RuleFor(request => request.PhotoGalleryId)
                .NotEmpty().WithMessage(validationMessages.PhotoGalleryIdRequired);

            RuleFor(request => request.ImageFileId)
                .NotEmpty().WithMessage(validationMessages.ImageFileIdRequired);
        }
    }

    public class CreatePhotoItemMapperProfile : Profile
    {
        public CreatePhotoItemMapperProfile()
        {
            CreateMap<Request, Entities.PhotoItem>();
            CreateMap<Entities.PhotoItem, Response>();
        }
    }

    internal sealed class Handler(
        IPhotoItemRepository photoItemRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly CreatePhotoItemValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.PhotoItem>(request);

            await photoItemRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/photoItem", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Photo Item");
    }
}