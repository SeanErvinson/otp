using System.Security.Cryptography;
using System.Text;

namespace Otp.Core.Utils;

public static class CryptoUtil
{
	public static string HashKey(string value)
	{
		using var sha256Hash = SHA256.Create();
		var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
		var sb = new StringBuilder();
		foreach (var hashedByte in hashedBytes) sb.Append(hashedByte.ToString("x2"));

		return sb.ToString();
	}

	public static string GenerateKey()
	{
		var key = new byte[256 / 8];
		using var generator = RandomNumberGenerator.Create();
		generator.GetBytes(key);
		return Convert.ToBase64String(key);
	}
}