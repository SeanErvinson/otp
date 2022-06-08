using Microsoft.EntityFrameworkCore;

namespace Otp.Application.Common.Models;

public class CursorResult<T> where T : new()
{
	private CursorResult(string before, string after, IEnumerable<T> items)
	{
		Before = before;
		After = after;
		Items = items;
	}

	public static async Task<CursorResult<T>> CreateAsync(IQueryable<T> source, int count, Func<T, string> cursorFunction)
	{
		var items = await source.Take(count).ToListAsync();
		return new CursorResult<T>(cursorFunction.Invoke(items.First()), cursorFunction.Invoke(items.Last()), items);
	}

	public bool HasBefore { get; }
	public bool HasAfter { get; }
	public string Before { get; }
	public string After { get; }
	public IEnumerable<T> Items { get; }
}