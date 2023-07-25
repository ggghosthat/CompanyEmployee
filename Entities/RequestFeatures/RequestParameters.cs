namespace Entities.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 5;

    public int PageNumber {get; set; } = 1;

    private int _pageSize = 2;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    public string OrderBy {get; set;}
}
