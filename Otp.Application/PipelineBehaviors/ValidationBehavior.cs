using FluentValidation;
using MediatR;
using Otp.Application.Common.Exceptions;

namespace Otp.Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request,
		CancellationToken cancellationToken,
		RequestHandlerDelegate<TResponse> next)
	{
		if (_validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);
			var validationResults = await Task.WhenAll(_validators.Select(v =>
				v.ValidateAsync(context, cancellationToken)));
			var failures = validationResults
				.Where(r => r.Errors.Any())
				.SelectMany(r => r.Errors)
				.ToList();

			if (failures.Any())
			{
				var validation = failures.GroupBy(error => error.PropertyName)
					.ToDictionary(e => e.Key, failures => failures.Select(c => c.ErrorMessage));

				throw new InvalidRequestException(ExceptionConstants.InvalidInput, "One or more validation errors occurred.", validation);
			}
		}
		return await next();
	}
}