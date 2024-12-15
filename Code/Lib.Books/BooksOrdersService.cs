using Flurl;
using Lib.ApiClient.Abstractions;
using Lib.Books.Abstractions;
using Lib.Books.Models.Order;
using Lib.Results;
using Microsoft.Extensions.Options;

namespace Lib.Books;

public sealed class BooksOrdersService : IBooksOrdersService
{
    private const string OrdersEndpoint = "/api/orders";

    private readonly IApiClient _apiClient;
    private readonly BooksApiOptions _apiOptions;

    public BooksOrdersService(IApiClient apiClient, IOptions<BooksApiOptions> apiOptions)
    {
        _apiClient = apiClient;
        _apiOptions = apiOptions.Value;
    }

    public Task<Result<IEnumerable<Order>>> GetOrders(uint pageIndex, uint pageSize)
    {
        var uri = BuildUri(pageIndex, pageSize);

        return _apiClient.Get<IEnumerable<Order>>(uri, "token");
    }

    private Uri BuildUri(uint pageIndex, uint pageSize)
    {
        var url = Url.Combine(_apiOptions.ApiUrl, OrdersEndpoint).SetQueryParams(new { pageIndex, pageSize });

        return new Uri(url);
    }
}
