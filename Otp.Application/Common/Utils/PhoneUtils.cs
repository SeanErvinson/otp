namespace Otp.Application.Common.Utils;

public static class PhoneUtils
{
	public static string ExtractCountryCode(string phoneNumber) => phoneNumber[1..3];
}