using MediatR;
using Otp.Application.Metric.Queries.GetMetric;

namespace Otp.Application.Metric.Metrics;

public static class MetricStrategyFactory
{
	public static IRequest<MetricData> GetMetricStrategy(MetricQuery query)
	{
		if (query.MetricName == SmsUsageCountMetric.MetricName)
		{
			return new SmsUsageCountMetric(query.StartDateTime, query.EndDateTime);
		}

		if (query.MetricName == EmailUsageCountMetric.MetricName)
		{
			return new EmailUsageCountMetric(query.StartDateTime, query.EndDateTime);
		}

		if (query.MetricName == ChannelUsageCountMetric.MetricName)
		{
			return new ChannelUsageCountMetric(query.StartDateTime, query.EndDateTime, query.MetricInterval);
		}
		throw new NotSupportedException($"Metric strategy '{query.MetricName}' is not supported");
	}
}

public record MetricQuery(string MetricName,
	DateTime StartDateTime,
	DateTime EndDateTime,
	MetricInterval? MetricInterval,
	Guid? ResourceId);