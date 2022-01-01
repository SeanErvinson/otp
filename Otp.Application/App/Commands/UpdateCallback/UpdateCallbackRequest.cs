namespace Otp.Application.App.Commands.UpdateCallback;

public record UpdateCallbackRequest(string CallbackUrl, string? EndpointSecret);