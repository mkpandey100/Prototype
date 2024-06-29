namespace Prototype.Lord.Application.Common;

public abstract class PageQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string OrderByColumn { get; set; }
    public string SearchValue { get; set; }

    internal int Skip
    {
        get
        {
            return (PageNumber - 1) * PageSize;
        }
    }
}