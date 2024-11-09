namespace Web.Api.Features.HealthActivity;

public class GetHealthActivity : IFeature
{
    public record Request(Guid Id) : IRequest<IDataResult<Response>>;

    public record Response(
        Guid Id,
        ActivityType ActivityType,
        int Calories,
        DateTime ActionStartDate,
        DateTime ActionEndDate,
        double Duration,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private class GetHealthActivityValidator : AbstractValidator<Request>
    {
        public GetHealthActivityValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(validationMessages.IdRequired);
        }
    }

    public class GetHealthActivityMapperProfile : Profile
    {
        public GetHealthActivityMapperProfile()
        {
            CreateMap<Request, Entities.HealthActivity>();
            CreateMap<Entities.HealthActivity, Response>();
        }
    }

    internal sealed class Handler(
        IHealthActivitiesRepository healthActivitiesRepository,
        PropertyValidationMessages validationMessages,
        IMapper mapper
    ) : IRequestHandler<Request, IDataResult<Response>>
    {
        private readonly GetHealthActivityValidator _validator = new(validationMessages);

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

            var entity = await healthActivitiesRepository.GetAsync(
                healthActivity => healthActivity.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var response = mapper.Map<Response>(entity);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapGet("/api/healthActivity/{id:guid}", async (Guid id, ISender sender) =>
        {
            var request = new Request(id);
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Health Activity");
    }
}