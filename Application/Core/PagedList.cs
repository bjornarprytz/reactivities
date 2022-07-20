
using Microsoft.EntityFrameworkCore;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)(Math.Ceiling(count / (double)pageSize));

        AddRange(items);
        
    }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}