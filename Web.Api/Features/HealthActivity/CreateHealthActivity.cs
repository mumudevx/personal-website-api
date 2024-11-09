namespace Web.Api.Features.HealthActivity;

public class CreateHealthActivity : IFeature
{
    public record Request(
        ActivityType ActivityType,
        int Calories,
        DateTime ActionStartDate,
        DateTime ActionEndDate
    ) : IRequest<IDataResult<Response>>;

    public record Response(Guid Id);

    private class CreateHealthActivityValidator : AbstractValidator<Request>
    {
        public CreateHealthActivityValidator(PropertyValidationMessages validationMessages)
        {
            RuleFor(request => request.ActivityType)
                .NotEmpty().WithMessage(validationMessages.ActivityTypeRequired);

            RuleFor(request => request.Calories)
                .NotEmpty().WithMessage(validationMessages.CaloriesRequired);

            RuleFor(request => request.ActionStartDate)
                .NotEmpty().WithMessage(validationMessages.ActionStartDateRequired);

            RuleFor(request => request.ActionEndDate)
                .NotEmpty().WithMessage(validationMessages.ActionEndDateRequired);
        }
    }

    public class CreateHealthActivityMapperProfile : Profile
    {
        public CreateHealthActivityMapperProfile()
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
        private readonly CreateHealthActivityValidator _validator = new(validationMessages);

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

            var entity = mapper.Map<Entities.HealthActivity>(request);

            await healthActivitiesRepository.AddAsync(entity, cancellationToken);

            var response = new Response(entity.Id);

            return new SuccessDataResult<Response>(response);
        }
    }

    public RouteHandlerBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        return app.MapPost("/api/healthActivity", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return !result.Success ? Results.NotFound(result.Message) : Results.Ok(result.Data);
        }).WithTags("Health Activity");
    }
}