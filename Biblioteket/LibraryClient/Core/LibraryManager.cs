using Biblioteket.LibraryClient.Models;
using Biblioteket.LibraryClient.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteket.LibraryClient.Core
{
    public static class LibraryManager
    {
        private static string GetValidInput(string prompt, Func<string, bool> validator)
        {
            while (true)
            {
                string input = ConsoleHelper.EscapeToMenu(prompt);
                if (input == "<ESC>") return null;

                if (validator(input))
                {
                    return input;
                }
            }
        }

        public static void RegisterBook()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Bok");

            using (var db = new LibraryDbContext())
            {
                int selectedAuthorId = 0;
                int selectedGenreId = 0;
                int selectedPublisherId = 0;

                while (true)
                {
                    var authors = db.Authors.OrderBy(a => a.FirstName).ToList();
                    string[] authorHeaders = { "ID", "Författare" };

                    List<string[]> authorRows = authors
                        .Select(a => new string[] { a.AuthorId.ToString(), $"{a.FirstName} {a.LastName}" })
                        .ToList();

                    ConsoleHelper.PrintTable(authorHeaders, authorRows);

                    Console.WriteLine("[N] - Ny författare");
                    Console.WriteLine("--------------------\n");
                    string input = ConsoleHelper.EscapeToMenu("Ange författar-ID eller 'N'");
                    if (input == "<ESC>") return;

                    if (input.ToUpper() == "N")
                    {
                        string firstName = GetValidInput("Förnamn", s => Inputvalidator.isValidString(s, 50, false));
                        if (firstName == null) return;

                        string lastName = GetValidInput("Efternamn", s => Inputvalidator.isValidString(s, 50, false));
                        if (lastName == null) return;

                        var newAuthor = new Author { FirstName = firstName, LastName = lastName };

                        db.Authors.Add(newAuthor);
                        db.SaveChanges();
                        selectedAuthorId = newAuthor.AuthorId;

                        ConsoleHelper.WriteSuccess($"Ny författare skapad: {firstName} {lastName} (ID: {selectedAuthorId})");
                        ConsoleHelper.ReadKeyMessage("Tryck enter för att fortsätta med boken");
                        break;
                    }
                    else if (int.TryParse(input, out int authorId) && db.Authors.Any(a => a.AuthorId == authorId))
                    {
                        selectedAuthorId = authorId;
                        break;
                    }
                    else
                    {
                        ConsoleHelper.WriteError("Ogiltigt val. Ange ett ID eller 'N'");
                    }

                    ConsoleHelper.ResetScreen();
                }

                while (true)
                {
                    ConsoleHelper.ResetScreen();
                    ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Bok");

                    var genres = db.Genres.OrderBy(g => g.GenreName).ToList();
                    string[] genreHeaders = { "ID", "Genre" };
                    List<string[]> genreRows = genres
                        .Select(g => new string[] { g.GenreId.ToString(), g.GenreName })
                        .ToList();

                    ConsoleHelper.PrintTable(genreHeaders, genreRows);

                    Console.WriteLine("[N] - Ny genre");
                    Console.WriteLine("------------\n");
                    string input = ConsoleHelper.EscapeToMenu("Ange Genre-ID eller 'N'");
                    if (input == "<ESC>") return;

                    if (input.ToUpper() == "N")
                    {
                        string genreName = GetValidInput("Namn på ny genre", s => Inputvalidator.isValidString(s, 50, false));
                        if (genreName == null) return;

                        var newGenre = new Genre { GenreName = genreName };

                        db.Genres.Add(newGenre);
                        db.SaveChanges();
                        selectedGenreId = newGenre.GenreId;

                        ConsoleHelper.WriteSuccess($"Skapad genre: {genreName} (ID: {selectedGenreId})");
                        ConsoleHelper.ReadKeyMessage("Tryck enter för att fortsätta med boken");
                        break;
                    }
                    else if (int.TryParse(input, out int genreId) && db.Genres.Any(g => g.GenreId == genreId))
                    {
                        selectedGenreId = genreId;
                        break;
                    }
                    else
                    {
                        ConsoleHelper.WriteError("Ogiltigt val. Ange ett ID eller 'N'");
                    }

                    ConsoleHelper.ResetScreen();
                }

                while (true)
                {
                    ConsoleHelper.ResetScreen();
                    ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Bok");

                    var publishers = db.Publishers.OrderBy(p => p.PublisherName).ToList();
                    string[] pubHeaders = { "ID", "Förlag" };
                    List<string[]> pubRows = publishers
                        .Select(p => new string[] { p.PublisherId.ToString(), p.PublisherName })
                        .ToList();

                    ConsoleHelper.PrintTable(pubHeaders, pubRows);

                    Console.WriteLine("[N] - Nytt förlag");
                    Console.WriteLine("--------------------\n");
                    string input = ConsoleHelper.EscapeToMenu("Ange Förlags-ID eller 'N'");
                    if (input == "<ESC>") return;

                    if (input.ToUpper() == "N")
                    {
                        string pubName = GetValidInput("Namn på nytt förlag", s => Inputvalidator.isValidString(s, 50, false));
                        if (pubName == null) return;

                        var newPublisher = new Publisher { PublisherName = pubName };
                        db.Publishers.Add(newPublisher);
                        db.SaveChanges();
                        selectedPublisherId = newPublisher.PublisherId;

                        ConsoleHelper.WriteSuccess($"Skapade förlag: {pubName} (ID: {selectedPublisherId})");
                        ConsoleHelper.ReadKeyMessage("Tryck enter för att fortsätta med boken");
                        break;
                    }
                    else if (int.TryParse(input, out int pubId) && db.Publishers.Any(p => p.PublisherId == pubId))
                    {
                        selectedPublisherId = pubId;
                        break;
                    }
                    else
                    {
                        ConsoleHelper.WriteError("Ogiltigt val. Ange ett ID eller 'N'");
                    }

                    ConsoleHelper.ResetScreen();
                }

                ConsoleHelper.ResetScreen();
                ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Bok");
                ConsoleHelper.SectionHeader(">>> Slutför Bokregistreringen <<<");

                string bookTitle = GetValidInput("Titel", s => Inputvalidator.isValidString(s, 50));
                if (bookTitle == null) return;

                string isbn = GetValidInput("ISBN (10/13 siffror)", Inputvalidator.isValidIsbn);
                if (isbn == null) return;

                try
                {
                    var book = new Book
                    {
                        Title = bookTitle,
                        Isbn = isbn,
                        AuthorId = selectedAuthorId,
                        GenreId = selectedGenreId,
                        PublisherId = selectedPublisherId,
                        Language = "Svenska",
                        PublicationDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    db.Books.Add(book);
                    db.SaveChanges();

                    ConsoleHelper.WriteSuccess($"Boken '{bookTitle}' är nu registrerad i systemet!");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"Ett fel uppstod när boken skulle sparas: {ex.Message}");
                }
            }
            ConsoleHelper.ReadKeyMessage();
        }

        public static void RegisterMember()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Medlem");

            string firstName = GetValidInput("Förnamn", s => Inputvalidator.isValidString(s, 50, false));
            if (firstName == null) return;

            string lastName = GetValidInput("Efternamn", s => Inputvalidator.isValidString(s, 50, false));
            if (lastName == null) return;

            string phoneNumber = GetValidInput("Telefonnummer (10 siffror)", Inputvalidator.isValidPhoneNumber);
            if (phoneNumber == null) return;

            string email = GetValidInput("E-post", Inputvalidator.isValidEmail);
            if (email == null) return;

            using (var db = new LibraryDbContext())
            {
                try
                {
                    var member = new Member
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    db.Members.Add(member);
                    db.SaveChanges();

                    Console.WriteLine();
                    ConsoleHelper.WriteSuccess("Medlem registrerad!");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"Ett fel uppstod när medlem skulle sparas: {ex.Message}");
                }
            }
            ConsoleHelper.ReadKeyMessage();
        }

        public static void RegisterLoan()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Lån");

            using (var db = new LibraryDbContext())
            {

                var members = db.Members
                    .OrderBy(m => m.FirstName)
                    .ToList();

                if (!members.Any())
                {
                    ConsoleHelper.WriteWarning("Det finns inga registrerade medlemmar");
                    ConsoleHelper.ReadKeyMessage();
                    return;
                }

                var memberRows = members.Select(m => new string[]
                {
                    m.MemberId.ToString(),
                    m.FirstName,
                    m.LastName,
                    m.Email,
                    m.PhoneNumber ?? "-"
                }).ToList();

                string[] memberHeaders = { "ID", "Förnamn", "Efternamn", "E-post", "Telefon" };
                ConsoleHelper.PrintTable(memberHeaders, memberRows, row => ConsoleColor.White);

                int selectedMemberId = 0;

                while (true)
                {
                    string? input = ConsoleHelper.EscapeToMenu("Ange Medlems-ID");
                    if (input == "<ESC>") return;

                    if (!Inputvalidator.isValidNumber(input)) continue;

                    int id = int.Parse(input);
                    if (members.Any(m => m.MemberId == id))
                    {
                        selectedMemberId = id;
                        break;
                    }
                    else
                    {
                        ConsoleHelper.WriteError("Ogiltigt Medlem-ID");
                    }
                }

                ConsoleHelper.ResetScreen();
                ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Registrera Lån");
                ConsoleHelper.WriteInfo($"Vald medlem ID: {selectedMemberId}");
                Console.WriteLine();

                var loanedBookIds = db.Loans
                    .Where(l => l.ReturnDate == null)
                    .Select(l => l.BookId)
                    .ToList();


                var availableBooks = db.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Where(b => !loanedBookIds.Contains(b.BookId))
                    .OrderBy(b => b.Title)
                    .ToList();

                if (!availableBooks.Any())
                {
                    ConsoleHelper.WriteWarning("Det finns inga tillgängliga böcker att låna just nu");
                    ConsoleHelper.ReadKeyMessage();
                    return;
                }

                var bookRows = availableBooks.Select(b => new string[]
                {
                    b.BookId.ToString(),
                    b.Title.Length > 30 ? b.Title.Substring(0, 27) + "..." : b.Title,
                    $"{b.Author.FirstName} {b.Author.LastName}",
                    b.Genre?.GenreName ?? "Okänd"
                }).ToList();

                string[] bookHeaders = { "ID", "Titel", "Författare", "Genre" };
                ConsoleHelper.PrintTable(bookHeaders, bookRows, row => ConsoleColor.White);

                int selectedBookId = 0;

                while (true)
                {
                    string? input = ConsoleHelper.EscapeToMenu("Ange Bok-ID");
                    if (input == "<ESC>") return;

                    if (!Inputvalidator.isValidNumber(input)) continue;

                    int id = int.Parse(input);
                    if (availableBooks.Any(b => b.BookId == id))
                    {
                        selectedBookId = id;
                        break;
                    }
                    else
                    {
                        ConsoleHelper.WriteError("Ogiltigt Bok-ID");
                    }
                }
                try
                {
                    var loan = new Loan
                    {
                        MemberId = selectedMemberId,
                        BookId = selectedBookId,
                        LoanDate = DateOnly.FromDateTime(DateTime.Now),
                        DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                        ReturnDate = null
                    };

                    db.Loans.Add(loan);
                    db.SaveChanges();

                    var bookTitle = availableBooks.First(b => b.BookId == selectedBookId).Title;

                    ConsoleHelper.WriteSuccess($"\nLån registrerat!\nBok: {bookTitle}\nÅterlämnas senast: {loan.DueDate}");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"Ett fel uppstod: {ex.Message}");
                }
            }
            ConsoleHelper.ReadKeyMessage();
        }

        public static void RegisterReturn()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Återlämna Bok");

            using (var db = new LibraryDbContext())
            {
                var activeLoans = db.Loans
                    .Include(l => l.Book)
                        .ThenInclude(b => b.Genre)
                    .Include(l => l.Member)
                    .Where(l => l.ReturnDate == null)
                    .OrderBy(l => l.DueDate)
                    .ToList();

                if (!activeLoans.Any())
                {
                    ConsoleHelper.WriteInfo("Det finns inga utlånade böcker att återlämna just nu.");
                    ConsoleHelper.ReadKeyMessage();
                    return;
                }

                int overdueCount = 0;
                var today = DateOnly.FromDateTime(DateTime.Now);
                var rows = new List<string[]>();

                foreach (var l in activeLoans)
                {

                    string loanDate = l.LoanDate.HasValue
                        ? l.LoanDate.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd")
                        : "-";

                    string dueDate = l.DueDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd");

                    rows.Add(new string[]
                    {
                        l.BookId.ToString(),
                        l.Book.Title,
                        l.Book.Genre?.GenreName ?? "Okänd",
                        $"{l.Member.FirstName} {l.Member.LastName}",
                        loanDate,
                        dueDate
                    });
                }

                ConsoleColor RowColorFunc(string[] row)
                {
                    var dueDate = DateOnly.Parse(row[5]);
                    int daysToDue = dueDate.DayNumber - today.DayNumber;

                    if (dueDate < today)
                    {
                        overdueCount++;
                        return ConsoleColor.Red;
                    }
                    else if (daysToDue <= 3)
                    {
                        return ConsoleColor.Yellow;
                    }
                    else
                    {
                        return ConsoleColor.Green;
                    }
                }

                string[] headers = { "ID", "Boktitel", "Genre", "Lånad av", "Lånad datum", "Förfaller" };
                ConsoleHelper.PrintTable(headers, rows, RowColorFunc);

                Loan? loanToUpdate = null;

                while (true)
                {
                    string? input = ConsoleHelper.EscapeToMenu("Ange Bok-ID som ska återlämnas");
                    if (input == "<ESC>") return;

                    if (!Inputvalidator.isValidNumber(input)) continue;

                    int bookId = int.Parse(input);
                    loanToUpdate = activeLoans.FirstOrDefault(l => l.BookId == bookId);

                    if (loanToUpdate != null) break;
                    else
                    {
                        ConsoleHelper.WriteWarning("Ogiltigt Bok-ID. Kontrollera listan ovan.");
                    }
                }

                try
                {
                    loanToUpdate.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                    db.SaveChanges();

                    if (loanToUpdate.ReturnDate > loanToUpdate.DueDate)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        ConsoleHelper.WriteSuccess($"Boken '{loanToUpdate.Book.Title}' återlämnades för sent!");
                    }
                    else
                    {
                        ConsoleHelper.WriteSuccess($"Boken '{loanToUpdate.Book.Title}' är nu återlämnad.");
                    }
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"Ett fel uppstod vid sparandet: {ex.Message}");
                }
            }

            ConsoleHelper.ReadKeyMessage();
        }

        public static void ListAllActiveLoans()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Aktiva Lån");

            using (var db = new LibraryDbContext())
            {
                var loans = db.Loans
                    .Include(l => l.Book)
                        .ThenInclude(b => b.Genre)
                    .Include(l => l.Member)
                    .Where(l => l.ReturnDate == null)
                    .OrderBy(l => l.DueDate)
                    .ToList();

                if (!loans.Any())
                {
                    ConsoleHelper.WriteInfo("Det finns inga aktiva lån i systemet just nu");
                    ConsoleHelper.ReadKeyMessage();
                    return;
                }

                int overdueCount = 0;
                var today = DateOnly.FromDateTime(DateTime.Now);
                var rows = new List<string[]>();

                foreach (var l in loans)
                {
                    string loanDate = l.LoanDate.HasValue
                        ? l.LoanDate.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd")
                        : "-";
                    string dueDate = l.DueDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd");

                    rows.Add(new string[]
                    {
                        l.LoanId.ToString(),
                        l.Book.Title,
                        l.Book.Genre?.GenreName ?? "Okänd",
                        $"{l.Member.FirstName} {l.Member.LastName}",
                        loanDate,
                        dueDate
                    });
                }

                ConsoleColor RowColorFunc(string[] row)
                {
                    var dueDate = DateOnly.Parse(row[5]);
                    int daysToDue = dueDate.DayNumber - today.DayNumber;

                    if (dueDate < today)
                    {
                        overdueCount++;
                        return ConsoleColor.Red;
                    }
                    else if (daysToDue <= 3)
                    {
                        return ConsoleColor.Yellow;
                    }
                    else
                    {
                        return ConsoleColor.Green;
                    }
                }

                string[] headers = { "ID", "Boktitel", "Genre", "Lånad av", "Lånad datum", "Förfaller" };
                ConsoleHelper.PrintTable(headers, rows, RowColorFunc);

                Console.WriteLine($"Totalt antal utlånade böcker: {loans.Count}");
                if (overdueCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"VARNING: {overdueCount} bok/böcker är försenade!");
                    Console.ResetColor();
                }
            }

            ConsoleHelper.ReadKeyMessage();
        }

        public static void SearchBooks()
        {
            ConsoleHelper.CrumbBar("Bibliotek", "Admin", "Sök bok");

            string searchInput;

            do
            {
                searchInput = ConsoleHelper.EscapeToMenu("Sökord (Titel, Författare eller ISBN)");
                if (searchInput == "<ESC>") return;
            } while (!Inputvalidator.isValidString(searchInput, 100, allowNumbers: true));

            Console.WriteLine();

            using (var db = new LibraryDbContext())
            {
                var books = db.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Include(b => b.Publisher)
                    .Where(b => b.Title.Contains(searchInput) ||
                                b.Isbn.Contains(searchInput) ||
                                b.Author.FirstName.Contains(searchInput) ||
                                b.Author.LastName.Contains(searchInput))
                    .ToList();

                if (books.Count > 0)
                {
                    ConsoleHelper.WriteInfo($"Hittade {books.Count} bok/böcker som matchar '{searchInput}':");
                    Console.WriteLine();

                    foreach (var b in books)
                    {
                        bool isLoaned = db.Loans.Any(l => l.BookId == b.BookId && l.ReturnDate == null);

                        Console.Write($"ID: {b.BookId} | Titel: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(b.Title);
                        Console.ResetColor();

                        Console.WriteLine($"   Författare: {b.Author.FirstName} {b.Author.LastName}");
                        Console.WriteLine($"   Genre: {b.Genre.GenreName} | Förlag: {b.Publisher.PublisherName}");
                        Console.WriteLine($"   ISBN: {b.Isbn}");

                        Console.Write("   Status: ");
                        if (isLoaned)
                        {
                            var loan = db.Loans.FirstOrDefault(l => l.BookId == b.BookId && l.ReturnDate == null);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"UTLÅNAD (Återlämnas senast: {loan?.DueDate})");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("TILLGÄNGLIG");
                        }
                        Console.ResetColor();
                        Console.WriteLine(new string('-', 60));
                    }
                }
                else
                {
                    ConsoleHelper.WriteWarning($"Inga böcker hittades som matchar '{searchInput}'.");
                }
            }
            ConsoleHelper.ReadKeyMessage();
        }
    }
}
