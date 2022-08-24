using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Otp.Application.Common.Models;

public class CursorResult<T> where T : class
{
	private CursorResult(string? before,
		string? after,
		IEnumerable<T> items)
	{
		Before = before;
		After = after;
		Items = items;
	}

	/// <summary>
	/// Create cursor result.
	/// </summary>
	/// <param name="source">A sequence of items to return.</param>
	/// <param name="count">The number of items returned.</param>
	/// <param name="orderBy">A function to extract a key from an element.</param>
	/// <param name="cursorFunc">A function that defines how the cursor is generated.</param>
	/// <typeparam name="TKey">The order property of the data source.</typeparam>
	/// <returns>Cursor result</returns>
	public static async Task<CursorResult<T>> CreateAsync<TKey>(IQueryable<T> source,
		int count,
		Func<T, TKey> orderBy,
		Func<T?, T?, (T?, T?)> beforeAfterFunc,
		Func<T, ICursor?> cursorFunc)
	{
		var totalItems = await source.Take(count).ToArrayAsync();
		var minItemCount = Math.Min(totalItems.Length, count);
		var items = totalItems[..minItemCount].OrderByDescending(orderBy).ToList();
		var firstItem = items.FirstOrDefault();
		var lastItem = items.LastOrDefault();
		var (before, after) = beforeAfterFunc(firstItem, lastItem);
		return new CursorResult<T>(GenerateCursor(before is null ? null : cursorFunc.Invoke(before)),
			GenerateCursor(after is null ? null : cursorFunc.Invoke(after)),
			items);
	}

	public string? Before { get; }
	public string? After { get; }
	public IEnumerable<T> Items { get; }

	private static string? GenerateCursor<TCursor>(TCursor? cursor) where TCursor : ICursor =>
		cursor is null
			? null
			: JsonConvert.SerializeObject(cursor); //TODO Replace once Text.JsonSerializer supports inheritance parsing
}

public interface ICursor
{
};