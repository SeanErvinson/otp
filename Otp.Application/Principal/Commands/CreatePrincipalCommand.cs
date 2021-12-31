using MediatR;

namespace Otp.Application.Principal.Commands;

public record CreatePrincipalCommand : IRequest<CreatePrincipalCommandDto>
{
	public string ObjectId { get; init; }
	public string DisplayName { get; init; }
	
	public class Handler : IRequestHandler<CreatePrincipalCommand, CreatePrincipalCommandDto>
	{
		public async Task<CreatePrincipalCommandDto> Handle(CreatePrincipalCommand request, CancellationToken cancellationToken)
		{
			

			return new CreatePrincipalCommandDto();
		}
	}
}

public enum CreatePrincipalCommandActions
{
	Continue,
	ShowBlockPage,
	ValidationError
}

public record CreatePrincipalCommandDto()
{
	public string Version { get; } = "1.0.0";
	public CreatePrincipalCommandActions Action { get; init; }
	public string? UserMessage { get; init; }
	public int? StatusCode { get; init; }
}