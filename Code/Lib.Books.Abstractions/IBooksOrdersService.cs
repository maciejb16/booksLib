using Lib.Books.Models.Order;
using Lib.Results;

namespace Lib.Books.Abstractions;

public interface IBooksOrdersService
{
    Task<Result<IEnumerable<Order>>> GetOrders(uint pageIndex, uint pageSize);
}
