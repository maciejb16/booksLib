using Lib.ApiClient.Abstractions;
using Lib.Books.Models.Order;
using Lib.Results;
using Microsoft.Extensions.Options;
using Moq;

namespace Lib.Books.UnitTests;

[TestFixture]
public class BooksOrdersServiceTests
{
    private Mock<IApiClient> _apiClientMock;

    private BooksOrdersService _service;

    [SetUp]
    public void Setup()
    {
        _apiClientMock = new Mock<IApiClient>();

        var options = Options.Create(new BooksApiOptions { ApiUrl = "http://localhost" });

        _service = new BooksOrdersService(_apiClientMock.Object, options);
    }

    [Test]
    public async Task GetBooks_ShouldReturnProperResult()
    {
        var expectedResult = new Result<IEnumerable<Order>>(new List<Order> { new() { OrderId = Guid.NewGuid() } });
        _apiClientMock
            .Setup(
                x => x.Get<IEnumerable<Order>>(
                    new Uri("http://localhost/api/orders?pageIndex=1&pageSize=100"),
                    "token")).ReturnsAsync(expectedResult);

        var result = await _service.GetOrders(1, 100);

        Assert.AreEqual(expectedResult, result);
    }
}