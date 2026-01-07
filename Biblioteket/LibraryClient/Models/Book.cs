using System;
using System.Collections.Generic;

namespace Biblioteket.LibraryClient.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Language { get; set; }

    public DateOnly PublicationDate { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual Publisher Publisher { get; set; } = null!;
}
