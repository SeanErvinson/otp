using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

/// <summary>
/// Price reference based on where the user is located.
/// </summary>
public class SmsPrice : TimedEntity<int>
{
	/// <summary>
	/// Country of which the request was requested from.
	/// </summary>
	public string OriginCountry { get; private set; }

	/// <summary>
	/// The country of which the request was requested to.
	/// </summary>
	public string RecipientCountry { get; private set; }
	
	/// <summary>
	/// This is the price per SMS. Adapt to whatever the major currency of the origin country is.
	/// Example: User is in US, price will then be in USD
	/// </summary>
	public float Price { get; private set; }
}