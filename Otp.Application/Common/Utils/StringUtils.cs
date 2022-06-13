using System.Runtime.CompilerServices;
using System.Text;

namespace Otp.Application.Common.Utils;

public static class StringUtils
{
	public static string Base64Encode(string value, [CallerArgumentExpression("value")] string valueName = "")
	{
		ArgumentNullException.ThrowIfNull(value, nameof(valueName));
		var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
		return Convert.ToBase64String(valueBytes);
	}

	public static bool TryBase64Decode(string base64,
		out string value,
		[CallerArgumentExpression("base64")] string base64Name = "")
	{
		ArgumentNullException.ThrowIfNull(base64, nameof(base64Name));
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