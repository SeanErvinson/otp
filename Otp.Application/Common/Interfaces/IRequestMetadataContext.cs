namespace Otp.Application.Common.Interfaces;

public interface IRequestMetadataContext
{
	/// <summary>
	/// Country of which the request came from. Make sure it is ISO 3166 Alpha-2 code.
	/// </summary>
	public string? Country { get; }	
}