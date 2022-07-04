using System.Text.Json;
using MediatR;
using Otp.Application.Common.Exceptions;
using Otp.Application.Metric.Metrics;

namespace Otp.Application.Metric.Queries.GetMetric;

public enum MetricInterval
{
	Month,
	Day,
	Second
}

public record GetMetricQuery(string MetricName,
	string TimeSpan,
	MetricInterval? MetricInterval,
	Guid? ResourceId) : IRequest<GetMetricResponse>
{
	public class Handler : IRequestHandler<GetMetricQuery, GetMetricResponse>
	{
		private readonly IMediator _mediator;

		public Handler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<GetMetricResponse> Handle(GetMetricQuery request, CancellationToken cancellationToken)
		{
			var (startDateTime, endDateTime) = ParseTimeSpan(request.TimeSpan);
			var query = new MetricQuery(request.MetricName,
				startDateTime,
				endDateTime,
				request.MetricInterval,
				request.ResourceId);
			IRequest<MetricData> command;

			try
			{
				command = MetricStrategyFactory.GetMetricStrategy(query);
			}
			catch (NotSupportedException ex)
			{
				throw new InvalidRequestException(ex.Message);
			}
			var result = await _mediator.Send(command, cancellationToken);
			return new GetMetricResponse(request.MetricName, result.Data);
		}
	}

	private static (DateTime startDateTime, DateTime endDateTime) ParseTimeSpan(string timeSpan)
	{
		var timespanRangeText = timeSpan.Split("/");

		if (timespanRangeText.Length != 2)
		{
			throw new ArgumentException("Time span must consist of start and end", nameof(timeSpan));
		}

		if (!DateTime.TryParse(timespanRangeText[0], out var startDateTime) ||
			!DateTime.TryParse(timespanRangeText[1], out var endDateTime))
		{
			throw new ArgumentException("Time span contains an invalid date", nameof(timeSpan));
		}
		return (startDateTime, endDateTime);
	}
}

public record MetricData(JsonElement Data);
public record GetMetricResponse(string Name, JsonElement Data);