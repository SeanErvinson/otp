using FluentValidation;

namespace Otp.Application.App.Queries.GetApps;

public class GetAppsQueryValidator : AbstractValidator<GetAppsQuery>
{
	public GetAppsQueryValidator()
	{
		RuleFor(request => request.PageSize).InclusiveBetween(5, 100);
		RuleFor(request => request.PageIndex).GreaterThan(0);
	}
}