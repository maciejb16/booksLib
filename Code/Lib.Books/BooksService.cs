using Flurl;
using Lib.ApiClient.Abstractions;
using Lib.Books.Abstractions;
using Lib.Books.Models.Book;
using Lib.Results;
using Microsoft.Extensions.Options;

namespace Lib.Books;

public sealed class BooksService : IBooksService
{
    private const string BooksEndpoint = "/api/books";

    private readonly IApiClient _apiClient;
    private readonly BooksApiOptions _apiOptions;

    public BooksService(IApiClient apiClient, IOptions<BooksApiOptions> apiOptions)
    {
        _apiClient = apiClient;
        _apiOptions = apiOptions.Value;
    }

    public Task<Result<IEnumerable<Book>>> GetBooks()
    {
        var uri = BuildUri();

        return _apiClient.Get<IEnumerable<Book>>(uri, "token");
    }

    public Task<Result> AddBooks(IEnumerable<Book> books)
    {
        var uri = BuildUri();

        return _apiClient.Post(uri, "token", books);
    }

    private Uri BuildUri()
    {
        var url = Url.Combine(_apiOptions.ApiUrl, BooksEndpoint);

        return new Uri(url);
    }
}
