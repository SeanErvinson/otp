namespace Otp.Core.Utils;

public static class OtpUtil
{
	private static readonly Random Random = new();
	
	public static string GenerateCode(int length = 6)
	{
		if (length >= 10)
		{
			throw new ArgumentException("Length must be lower than 10");
		}
		
		var value = Random.Next((int)Math.Pow(10, length));
		return value.ToString(new string('0', length));
	}
}