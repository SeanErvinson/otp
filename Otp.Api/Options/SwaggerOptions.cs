namespace Otp.Api.Options;

public record SwaggerOptions
{
	public const string Section = "Swagger";
	public string Title { get; init; } = default!;
	public string Version { get; init; } = default!;
	public string Description { get; init; } = default!;
	public string Domain { get; init; } = default!;
}