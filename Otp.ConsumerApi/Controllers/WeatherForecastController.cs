using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Otp.Core.Utils;

namespace Otp.ConsumerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiverController : ControllerBase
{
	private readonly ILogger<ReceiverController> _logger;
	private const string SignatureHeaderKey = "OTP_SIGNATURE";
	private const string EndpointSecret = "Password";

	public ReceiverController(ILogger<ReceiverController> logger)
	{
		_logger = logger;
	}

	[HttpPost("no-secret")]
	public IActionResult NoSecret([FromBody] CallbackEventResponse response)
	{
		_logger.LogInformation("No Secret - Successfully decoded and received");
		return Ok(response);
	}
	
	[HttpPost("no-secret-check")]
	public IActionResult NoSecretButCheck([FromBody] CallbackEventResponse response)
	{
		var signature = HttpContext.Request.Headers[SignatureHeaderKey].First();
		var signatureResponse = JsonSerializer.Deserialize<SignatureResponse>(signature);
		var data = $"{signatureResponse.Timestamp}.{JsonSerializer.Serialize(response)}";
		var result = CryptoUtil.Hash256(data, null);
		
		if (result == signatureResponse.Value)
		{
			_logger.LogInformation("No Secret Check - Successfully decoded and received");
			return Ok(response);
		}
		
		return NotFound();
	}

	[HttpPost("secret")]
	public IActionResult Secret([FromBody] CallbackEventResponse response)
	{
		var signature = HttpContext.Request.Headers[SignatureHeaderKey].First();
		var signatureResponse = JsonSerializer.Deserialize<SignatureResponse>(signature);
		var data = $"{signatureResponse.Timestamp}.{JsonSerializer.Serialize(response)}";
		var result = CryptoUtil.Hash256(data, EndpointSecret);

		if (result == signatureResponse.Value)
		{
			_logger.LogInformation("Secret - Successfully decoded and received {Mode} {Contact} - {Type}", response.Contact, response.Mode, response.Type);
			return Ok(response);
		}
		
		_logger.LogInformation("Secret - Failed to decoded");
		return NotFound();
	}

	public record SignatureResponse(long Timestamp, string Value);
	
	public record CallbackEventResponse
	{
		public Guid RequestId { get; init; }
		public Mode Mode { get; init; }
		public string Contact { get; init; }
		public EventType Type { get; init; }
	};

	public enum Mode
	{
		SMS,
		Email
	}

	public enum EventType
	{
		Success,
		Failed,
		Canceled
	}
}