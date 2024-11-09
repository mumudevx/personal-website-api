namespace Web.Api.Features.PhotoItem;

public class GetPhotoItem : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        string Title,
        string? Description,
        int Order,
        Guid PhotoGalleryId,
        Guid ImageFileId,
        List<string> Tags,
        DateTime CreatedAt,
        DateTime? ModifiedAt
    );

    private class GetPhotoItemValidator : AbstractValidator<Request>
    {
        public GetPhotoItemValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetPhotoItemMapperProfile : Profile
    {
        public GetPhotoItemMapperProfile()
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
        private readonly GetPhotoItemValidator _validator = new(validationMessages);

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

            var entity = await photoItemRepository.GetAsync(
                photoItem => photoItem.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/photoItem/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Photo Item");
    }
}