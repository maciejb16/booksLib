namespace Lib.Books.Models.Order;

public record Order
{
    public Guid OrderId { get; init; }
    public List<OrderLine> OrderLines { get; init; }
}
