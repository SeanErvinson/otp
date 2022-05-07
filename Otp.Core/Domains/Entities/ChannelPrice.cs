using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class ChannelPrice : TimedEntity
{
	public int? Threshold { get; private set; }
	public double Price { get; private set; }
	public string Source { get; private set; }
	public string Destination { get; private set; }
	public Channel Channel { get; private set; }

	private ChannelPrice()
	{
	}

	public ChannelPrice(int? threshold, double price, string source, string destination, Channel channel)
	{
		Threshold = threshold;
		Price = price;
		Source = source;
		Destination = destination;
		Channel = channel;
	}
}