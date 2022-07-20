using Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PagingParams pagingParams)
    {
        return source.ToPagedListAsync(pagingParams.PageNumber, pagingParams.PageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}