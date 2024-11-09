namespace Web.Api.Features.ImageFile;

public class CreateImageFile : IFeature
{
    public record Request(
        string Name,
        string Alt,
        string Path,
        string? Description
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreateImageFileValidator : AbstractValidator<Request>
    {
        public CreateImageFileValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage(validationMessages.NameRequired);

            RuleFor(request => request.Alt)
                .NotEmpty().WithMessage(validationMessages.AltRequired);

            RuleFor(request => request.Path)
                .NotEmpty().WithMessage(validationMessages.PathRequired);

            RuleFor(request => request.Description)
                .MaximumLength(500).WithMessage(validationMessages.DescriptionMaxLength);
        }
    }

    public class CreateImageFileMapperProfile : Profile
    {
        public CreateImageFileMapperProfile()
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
        private readonly CreateImageFileValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.ImageFile>(request);

            await imageFileRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/imageFile", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Image File");
    }
}