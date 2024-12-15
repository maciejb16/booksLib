using Lib.ApiClient.Abstractions;
using Lib.Books.Models.Book;
using Lib.Results;
using Microsoft.Extensions.Options;
using Moq;

namespace Lib.Books.UnitTests;

[TestFixture]
public class BooksServiceTests
{
    private Mock<IApiClient> _apiClientMock;

    private BooksService _service;

    [SetUp]
    public void Setup()
    {
        _apiClientMock = new Mock<IApiClient>();

        var options = Options.Create(new BooksApiOptions { ApiUrl = "http://localhost" });

        _service = new BooksService(_apiClientMock.Object, options);
    }

    [Test]
    public async Task GetBooks_ShouldReturnProperResult()
    {
        var expectedResult = new Result<IEnumerable<Book>>(new List<Book> { new() { Id = 1 } });
        _apiClientMock
            .Setup(
                x => x.Get<IEnumerable<Book>>(
                    new Uri("http://localhost/api/books"),
                    "token")).ReturnsAsync(expectedResult);

        var result = await _service.GetBooks();

        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public async Task AddBooks_ShouldAddBooksAndReturnProperResult()
    {
        var expectedResult = Result.Error("error");
        var books = new List<Book> { new Book { Id = 1 } };
        _apiClientMock
            .Setup(
                x => x.Post(
                    new Uri("http://localhost/api/books"),
                    "token", books)).ReturnsAsync(expectedResult);

        var result = await _service.AddBooks(books);

        Assert.AreEqual(expectedResult, result);
    }
}