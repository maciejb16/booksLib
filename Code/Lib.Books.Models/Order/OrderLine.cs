namespace Lib.Books.Models.Order;

public record OrderLine
{
    public long BookId { get; init; }

    public uint Quantity { get; init; }
}
