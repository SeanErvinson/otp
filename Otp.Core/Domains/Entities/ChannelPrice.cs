using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class ChannelPrice : TimedEntity
{
	public string Origin { get; }
	public decimal SmsPrice { get; private set; }
	public decimal EmailPrice { get; private set; }
	public string Country { get; }

	private ChannelPrice()
	{
	}

	public ChannelPrice(string origin, decimal smsPrice, decimal emailPrice, string country)
	{
		Origin = origin;
		SmsPrice = smsPrice;
		EmailPrice = emailPrice;
		Country = country;
	}

	public void UpdateSmsPrice(decimal price)
	{
		SmsPrice = price;
	}
	
	public void UpdateEmailPrice(decimal price)
	{
		SmsPrice = price;
	}
}