using Lib.Books.Models.Book;
using Lib.Results;

namespace Lib.Books.Abstractions;

public interface IBooksService
{
    Task<Result<IEnumerable<Book>>> GetBooks();

    Task<Result> AddBooks(IEnumerable<Book> books);
}
