using System.Linq.Expressions;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using Otp.Application.Metric.Queries.GetMetric;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;
using Serilog.Context;

namespace Otp.Application.Metric.Metrics;

public record ChannelUsageCountMetric(DateTime StartDateTime,
									DateTime EndDateTime,
									MetricInterval? MetricInterval) : IRequest<MetricData>
{
	public static string MetricName => "ChannelUsageCount";

	public class Handler : IRequestHandler<ChannelUsageCountMetric, MetricData>
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


		public async Task<MetricData> Handle(ChannelUsageCountMetric request,
											CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("MetricName", nameof(ChannelUsageCountMetric)))
			{
				Expression<Func<OtpRequest, bool>> RequestSuccessFilter(DateTime startDateTime,
																		DateTime endDateTime)
				{
					return otpRequest =>
						otpRequest.CreatedAt >= startDateTime &&
						otpRequest.CreatedAt <= endDateTime &&
						otpRequest.Timeline.Any(@event =>
								@event.State == EventState.Deliver && @event.Status == EventStatus.Success) &&
						otpRequest.App.PrincipalId == _currentUserService.PrincipalId;
				}

				var successfulRequests = _applicationDbContext.OtpRequests
					.Where(RequestSuccessFilter(request.StartDateTime, request.EndDateTime))
					.OrderByDescending(otpRequest => otpRequest.CreatedAt);
				if (request.MetricInterval is Queries.GetMetric.MetricInterval.Day)
				{
					var completeDateTimes = DateUtils.RangeDays(request.StartDateTime, request.EndDateTime).ToList();
					var groupBy = successfulRequests.GroupBy(otpRequest => new
					{
						otpRequest.CreatedAt.Date
					});
					var smsTimeSeries = await groupBy.Select(c => new TimeSeries(c.Key.Date, c.Count(otp => otp.Channel == Channel.Sms)))
						.ToListAsync(cancellationToken);
					var emailTimeSeries = await groupBy.Select(c => new TimeSeries(c.Key.Date, c.Count(otp => otp.Channel == Channel.Email)))
						.ToListAsync(cancellationToken);
					PopulateMissingTimeSeries(completeDateTimes, smsTimeSeries);
					PopulateMissingTimeSeries(completeDateTimes, emailTimeSeries);

					return new MetricData(JsonSerializer.SerializeToElement(new ChannelUsageCountMetricResponse(smsTimeSeries.OrderBy(c => c.TimeStamp),
																				emailTimeSeries.OrderBy(c => c.TimeStamp)),
																			_jsonSerializerOptions));
				}
				else
				{
					var completeDateTimes = DateUtils.RangeMonths(request.StartDateTime, request.EndDateTime).ToList();
					var groupBy = successfulRequests.GroupBy(otpRequest => new
					{
						otpRequest.CreatedAt.Year,
						otpRequest.CreatedAt.Month
					});
					var smsTimeSeries = await groupBy
						.Select(c => new TimeSeries(new DateTime(c.Key.Year, c.Key.Month, 1), c.Count(otp => otp.Channel == Channel.Sms)))
						.ToListAsync(cancellationToken);
					var emailTimeSeries = await groupBy
						.Select(c => new TimeSeries(new DateTime(c.Key.Year, c.Key.Month, 1), c.Count(otp => otp.Channel == Channel.Email)))
						.ToListAsync(cancellationToken);

					PopulateMissingTimeSeries(completeDateTimes, smsTimeSeries);
					PopulateMissingTimeSeries(completeDateTimes, emailTimeSeries);

					return new MetricData(JsonSerializer.SerializeToElement(new ChannelUsageCountMetricResponse(smsTimeSeries.OrderBy(c => c.TimeStamp),
																				emailTimeSeries.OrderBy(c => c.TimeStamp)),
																			_jsonSerializerOptions));
				}
			}
		}

		private static void PopulateMissingTimeSeries(IEnumerable<DateTime> completeDateTimes, List<TimeSeries> timeSeries)
		{
			foreach (var dateTime in completeDateTimes)
				if (!timeSeries.Exists(c => c.TimeStamp.Date == dateTime.Date))
					timeSeries.Add(new TimeSeries(dateTime, 0));
		}
	}

	private record TimeSeries(DateTime TimeStamp,
							float Value);

	private record ChannelUsageCountMetricResponse(IEnumerable<TimeSeries> Sms, IEnumerable<TimeSeries> Email);

	public MetricData SerializeDate()
	{
		throw new NotImplementedException();
	}
}