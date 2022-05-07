using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

public class RequestInfo : ValueObject
{
	public string IpAddress { get; private set; }
	public string UserAgent { get; private set; }
	public string Referrer { get; private set; }
	

	public RequestInfo(string ipAddress, string userAgent, string referrer)
	{
		IpAddress = ipAddress;
		UserAgent = userAgent;
		Referrer = referrer;
	}
}