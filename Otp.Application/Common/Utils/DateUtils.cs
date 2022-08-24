using NodaTime;

namespace Otp.Application.Common.Utils;

public static class DateUtils
{
	public static IEnumerable<DateTime> RangeDays(DateTime start, DateTime end)
	{
		var period = CalculatePeriod(start, end, PeriodUnits.Days);
		var next = start;

		for (var i = 0; i < period.Days; i++)
		{
			next = next.AddDays(1);
			yield return next;
		}
	}

	public static IEnumerable<DateTime> RangeMonths(DateTime start, DateTime end)
	{
		var period = CalculatePeriod(start, end, PeriodUnits.Months);
		var next = start;

		for (var i = 0; i < period.Months; i++)
		{
			next = next.AddMonths(1);
			yield return next;
		}
	}

	private static Period CalculatePeriod(DateTime start, DateTime end, PeriodUnits units)
	{
		var startDate = LocalDateTime.FromDateTime(start);
		var endDate = LocalDateTime.FromDateTime(end);
		return Period.Between(startDate, endDate, units);
	}
}