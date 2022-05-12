using System.Linq.Expressions;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Application.Metric.Queries.GetMetric;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;
using Serilog.Context;

namespace Otp.Application.Metric.Metrics;

public record SmsUsageCountMetric(DateTime StartDateTime,
								DateTime EndDateTime) : IRequest<MetricData>
{
	public static string MetricName => "SmsUsageCount";

	public class Handler : IRequestHandler<SmsUsageCountMetric, MetricData>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public Handler(IApplicationDbContext applicationDbContext,
						ICurrentUserService currentUserService,
						JsonSerializerOptions jsonSerializerOptions)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		public async Task<MetricData> Handle(SmsUsageCountMetric request,
											CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("MetricName", nameof(SmsUsageCountMetric)))
			{
				Expression<Func<OtpRequest, bool>> SmsRequest(DateTime startDateTime,
															DateTime endDateTime)
				{
					return otpRequest =>
						otpRequest.Channel == Channel.Sms &&
						otpRequest.CreatedAt >= startDateTime &&
						otpRequest.CreatedAt <= endDateTime &&
						otpRequest.Status == OtpRequestStatus.Success &&
						otpRequest.App.PrincipalId == _currentUserService.PrincipalId;
				}

				var requests = await _applicationDbContext.OtpRequests.CountAsync(SmsRequest(request.StartDateTime, request.EndDateTime),
																				cancellationToken);

				var previousMonthRequests = await _applicationDbContext.OtpRequests.CountAsync(
					SmsRequest(request.StartDateTime.AddMonths(-1), request.EndDateTime.AddMonths(-1)),
					cancellationToken);

				var result = new SmsUsageCountMetricResult
				{
					SentRequest = requests,
					PreviousMonthSentRequest = previousMonthRequests
				};

				return new MetricData(JsonSerializer.SerializeToElement(result, _jsonSerializerOptions));
			}
		}
	}

	private record SmsUsageCountMetricResult
	{
		public int PreviousMonthSentRequest { get; init; }
		public int SentRequest { get; init; }
	}
}