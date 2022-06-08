using System.Runtime.CompilerServices;
using System.Text;

namespace Otp.Application.Common.Utils;

public static class StringUtils
{
	public static string Base64Encode(string value, [CallerArgumentExpression("value")] string valueName = "")
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentNullException(nameof(valueName), "Value is empty");
		}
		var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
		return Convert.ToBase64String(valueBytes);
	}

	public static bool TryBase64Decode(string base64,
		out string value,
		[CallerArgumentExpression("base64")] string base64Name = "")
	{
		if (string.IsNullOrWhiteSpace(base64))
		{
			throw new ArgumentNullException(nameof(base64Name), "Value is empty");
		}
		try
		{
			var bytes = Convert.FromBase64String(base64);
			value = Encoding.UTF8.GetString(bytes);
			return true;
		}
		catch (Exception)
		{
			value = string.Empty;
			return false;
		}
	}
}