using FluentValidation;

namespace Otp.Application.Logs.Queries.GetLogs;

public class GetLogsQueryValidator : AbstractValidator<GetLogsQuery>
{
	public GetLogsQueryValidator()
	{
		CascadeMode = CascadeMode.Stop;
		RuleFor(request => request.After)
			.Empty()
			.When(request => !string.IsNullOrWhiteSpace(request.Before))
			.WithMessage("Before cursor already specified");
		RuleFor(request => request.Before)
			.Empty()
			.When(request => !string.IsNullOrWhiteSpace(request.After))
			.WithMessage("After cursor already specified");
	}
}