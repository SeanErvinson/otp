using System.Text.Json;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender.EmailProviders;

public class AwsSesEmailProvider : IEmailProvider
{
	private const string SenderAddress = "ervinsonong@gmail.com";
	private const string ConfigurationSet = "ohtp-dev-configuration-set";
	private const string TemplateName = "OTPTemplate";

	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public AwsSesEmailProvider(JsonSerializerOptions jsonSerializerOptions)
	{
		_jsonSerializerOptions = jsonSerializerOptions;
	}

	public async Task Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		using var client = new AmazonSimpleEmailServiceV2Client(RegionEndpoint.APSoutheast1);
		var sendRequest = new SendEmailRequest
		{
			FromEmailAddress = SenderAddress,
			Destination = new Destination
			{
				ToAddresses =
					new List<string> { request.Recipient }
			},
			Content = new EmailContent
			{
				Template = new Template
				{
					TemplateName = TemplateName,
					TemplateData = JsonSerializer.Serialize(new TemplateData(request.Code,
							@"https://pages.getpostman.com/rs/067-UMD-991/images/pm-logo-horiz%402x.png",
							request.Recipient),
						_jsonSerializerOptions)
				}
			},
			EmailTags = new List<MessageTag> { new() { Name = nameof(request.AppId), Value = request.AppId.ToString() } },
			ConfigurationSetName = ConfigurationSet
		};

		try
		{
			Console.WriteLine("Sending email using Amazon SES...");
			var response = await client.SendEmailAsync(sendRequest, cancellationToken);
			Console.WriteLine("The email was sent successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine("The email was not sent.");
			Console.WriteLine("Error message: " + ex.Message);
		}
		Console.WriteLine($"Sending Email from SendGridProvider {request.Recipient}");
	}

	private record TemplateData(string Code, string LogoUrl, string ReceiverAddress);
}