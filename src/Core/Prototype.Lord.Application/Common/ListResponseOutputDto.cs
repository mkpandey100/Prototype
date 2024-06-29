namespace Prototype.Lord.Application.Common;

public class ListResponseOutputDto<T> : OutputDto
{
    public List<T> Data { get; set; }
    public int TotalRecord { get; set; }
}