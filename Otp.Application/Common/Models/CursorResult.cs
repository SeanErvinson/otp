using Microsoft.EntityFrameworkCore;

namespace Otp.Application.Common.Models;

public class CursorResult<T> where T : new()
{
	private CursorResult(
		bool hasBefore,
		bool hasAfter,
		string before,
		string after,
		IEnumerable<T> items)
	{
		HasAfter = hasAfter;
		HasBefore = hasBefore;
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
	/// <param name="hasPredicate">A function to compute if cursor has before and has after.</param>
	/// <param name="cursorFunc">A function that defines how the cursor is generated.</param>
	/// <typeparam name="TKey">The order property of the data source.</typeparam>
	/// <returns>Cursor result</returns>
	public static async Task<CursorResult<T>> CreateAsync<TKey>(IQueryable<T> source,
		int count,
		Func<T, TKey> orderBy,
		Func<T?, T?, (bool, bool)> hasPredicate,
		Func<T?, string> cursorFunc)
	{
		var totalItems = await source.Take(count).ToArrayAsync();
		var minItemCount = Math.Min(totalItems.Length, count);
		var items = totalItems[..minItemCount].OrderByDescending(orderBy).ToList();
		var firstItem = items.FirstOrDefault();
		var lastItem = items.LastOrDefault();
		var (hasBefore, hasAfter) = hasPredicate.Invoke(firstItem, lastItem);

		return new CursorResult<T>(hasBefore,
			hasAfter,
			cursorFunc.Invoke(lastItem),
			cursorFunc.Invoke(firstItem),
			items);
	}

	public bool HasBefore { get; }
	public bool HasAfter { get; }
	public string Before { get; }
	public string After { get; }
	public IEnumerable<T> Items { get; }
}