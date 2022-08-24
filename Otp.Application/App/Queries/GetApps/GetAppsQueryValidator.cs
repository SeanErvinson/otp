using FluentValidation;

namespace Otp.Application.App.Queries.GetApps;

public class GetAppsQueryValidator : AbstractValidator<GetApps>
{
	public GetAppsQueryValidator()
	{
		RuleFor(request => request.PageSize).InclusiveBetween(5, 100);
		RuleFor(request => request.PageIndex).GreaterThan(0);
	}
}