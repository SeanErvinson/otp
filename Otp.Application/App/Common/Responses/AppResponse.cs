namespace Otp.Application.App.Common.Responses;

public sealed record AppResponse
{
	public Guid Id { get; set; }
	public string Name { get; init; } = default!;
	public string? Description { get; init; }
	public string? BackgroundUrl { get; init; }
	public string? LogoUrl { get; init; }
	public IEnumerable<string>? Tags { get; init; }
	public string? CallbackUrl { get; init; }
	public DateTime CreatedAt { get; init; }
	public DateTime? UpdatedAt { get; init; }
}