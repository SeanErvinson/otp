using MediatR;

namespace Otp.Application.Principal.Commands;

public record CreatePrincipal : IRequest<CreatePrincipalResponse>
{
	public string ObjectId { get; init; }
	public string DisplayName { get; init; }
	
	public class Handler : IRequestHandler<CreatePrincipal, CreatePrincipalResponse>
	{
		public async Task<CreatePrincipalResponse> Handle(CreatePrincipal request, CancellationToken cancellationToken)
		{
			

			return new CreatePrincipalResponse();
		}
	}
}

public enum CreatePrincipalCommandActions
{
	Continue,
	ShowBlockPage,
	ValidationError
}

public record CreatePrincipalResponse()
{
	public string Version { get; } = "1.0.0";
	public CreatePrincipalCommandActions Action { get; init; }
	public string? UserMessage { get; init; }
	public int? StatusCode { get; init; }
}