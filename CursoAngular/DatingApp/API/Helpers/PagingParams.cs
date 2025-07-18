
namespace API.Helpers;

public class PagingParams
{
    private const int MaxPageSize = 50;
    public int pageNumber { get; set; }
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
