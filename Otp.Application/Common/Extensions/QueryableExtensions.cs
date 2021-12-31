namespace Otp.Application.Common.Extensions;

public static class QueryableExtensions
{
	public static Task<PaginatedResult<TDestination>> PaginatedResultAsync<TDestination>(this IQueryable<TDestination> queryable, int pageIndex, int pageSize)
		=> PaginatedResult<TDestination>.CreateAsync(queryable, pageIndex, pageSize);
}