using System.Linq.Expressions;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Application.Metric.Queries.GetMetric;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;
using Serilog.Context;

namespace Otp.Application.Metric.Metrics;

public sealed record EmailUsageCountMetric(DateTime StartDateTime,
	DateTime EndDateTime) : IRequest<MetricData>
{
	public static string MetricName => "EmailUsageCount";
}

public class EmailUsageCountMetricHandler : IRequestHandler<EmailUsageCountMetric, MetricData>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public EmailUsageCountMetricHandler(IApplicationDbContext applicationDbContext,
		ICurrentUserService currentUserService,
		JsonSerializerOptions jsonSerializerOptions)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
	}

	public async Task<MetricData> Handle(EmailUsageCountMetric request,
		CancellationToken cancellationToken)
	{
		using (LogContext.PushProperty("MetricName", nameof(EmailUsageCountMetric)))
		{
			Expression<Func<OtpRequest, bool>> SmsRequest(DateTime startDateTime,
				DateTime endDateTime)
			{
				return otpRequest =>
					otpRequest.Channel == Channel.Email &&
					otpRequest.CreatedAt >= startDateTime &&
					otpRequest.CreatedAt <= endDateTime &&
					otpRequest.Timeline.Any(@event =>
						@event.State == EventState.Deliver &&
						@event.Status == EventStatus.Success) &&
					otpRequest.App.PrincipalId == _currentUserService.PrincipalId;
			}

			var requests =
				await _applicationDbContext.OtpRequests.CountAsync(SmsRequest(request.StartDateTime, request.EndDateTime),
					cancellationToken);
			var previousMonthRequests =
				await _applicationDbContext.OtpRequests.CountAsync(SmsRequest(request.StartDateTime.AddMonths(-1),
						request.EndDateTime.AddMonths(-1)),
					cancellationToken);
			var result = new EmailUsageCountMetricResult(requests, previousMonthRequests);
			return new MetricData(JsonSerializer.SerializeToElement(result, _jsonSerializerOptions));
		}
	}
}

internal record EmailUsageCountMetricResult(int PreviousMonthSentRequest, int SentRequest);