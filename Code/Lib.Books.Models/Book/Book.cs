namespace Lib.Books.Models.Book;

public record Book
{
    public long Id { get; init; }

    public string Title { get; init; }

    public double Price { get; init; }

    public uint Bookstand { get; init; }

    public uint Shelf { get; init; }

    public List<Author> Authors { get; init; }
}
