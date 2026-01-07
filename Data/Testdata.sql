-- Testdata

-- Uppdatera med mer testdata

-- Lägg till förlag
-- Letar efter 'Libris' som PublisherName i tabellen Publishers, om den inte finns så skapas den
-- Samma med resterande
IF NOT EXISTS (SELECT 1 FROM Publishers WHERE PublisherName = 'Libris')
	INSERT INTO Publishers (PublisherName) VALUES ('Libris');

IF NOT EXISTS (SELECT 1 FROM Publishers WHERE PublisherName = 'Norstedts')
	INSERT INTO Publishers (PublisherName) VALUES ('Norstedts');

IF NOT EXISTS (SELECT 1 FROM Publishers WHERE PublisherName = 'Storyside')
	INSERT INTO Publishers (PublisherName) VALUES ('Storyside');

-- Lägg till genrer
-- Letar efter 'Skräck' som GenreName i tabellen Genres, om den inte finns så skapas den
-- Samma med resterande
IF NOT EXISTS (SELECT 1 FROM Genres WHERE GenreName = 'Skräck')
	INSERT INTO Genres (GenreName) VALUES ('Skräck');

IF NOT EXISTS (SELECT 1 FROM Genres WHERE GenreName = 'Epik')
	INSERT INTO Genres (GenreName) VALUES ('Epik');

IF NOT EXISTS (SELECT 1 FROM Genres WHERE GenreName = 'Äventyr')
	INSERT INTO Genres (GenreName) VALUES ('Äventyr');

-- Lägg till författare
-- Letar efter 'King' som LastName i tabellen Authors, om den inte finns så skapas den
-- Samma med resterande
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = 'King')
	INSERT INTO Authors (FirstName, LastName) VALUES ('Stephen', 'King');

IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = 'Kepler')
	INSERT INTO Authors (FirstName, LastName) VALUES ('Lars', 'Kepler');

IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = 'Lagerlöf')
	INSERT INTO Authors (FirstName, LastName) VALUES ('Selma', 'Lagerlöf');

-- Lägg till medlemmar
-- Letar efter 'Adam@gmail.com' som Email (unikt) i tabellen Members, om den inte finns så skapas den
-- Samma med resterande
IF NOT EXISTS (SELECT 1 FROM Members WHERE Email = 'Adam@gmail.com')
	INSERT INTO Members (FirstName, LastName, Email, PhoneNumber)
	VALUES ('Adam', 'Englund', 'Adam@gmail.com', '070-9834516');

IF NOT EXISTS (SELECT 1 FROM Members WHERE Email = 'Tobias@gmail.com')
	INSERT INTO Members (FirstName, LastName, Email, PhoneNumber)
	VALUES ('Tobias', 'Dewall', 'Tobias@gmail.com', '070-4759136');

IF NOT EXISTS (SELECT 1 FROM Members WHERE Email = 'Anton@gmail.com')
	INSERT INTO Members (FirstName, LastName, Email, PhoneNumber)
	VALUES ('Anton', 'Sund', 'Anton@gmail.com', '070-8512359');

-- Lägg till böcker
-- Letar efter '9789113084900' som ISBN i tabellen Books, om den inte finns så skapas den
-- SELECT TOP 1 används för FK som säkerställer korrekta relationer
-- Samma med resterande
IF NOT EXISTS (SELECT 1 FROM Books WHERE ISBN = '9789113084900')
BEGIN
	INSERT INTO Books (ISBN, Title, Language, PublicationDate, AuthorID, GenreID, PublisherID)
	VALUES (
		'9789113084900', 'Det (It)', 'Svenska', '1986-09-15',
		(SELECT TOP 1 AuthorID FROM Authors WHERE LastName = 'King'),
		(SELECT TOP 1 GenreID FROM Genres WHERE GenreName = 'Skräck'),
		(SELECT TOP 1 PublisherID FROM Publishers WHERE PublisherName = 'Libris')
	);
END

IF NOT EXISTS (SELECT 1 FROM Books WHERE ISBN = '9789113084813')
BEGIN
	INSERT INTO Books (ISBN, Title, Language, PublicationDate, AuthorID, GenreID, PublisherID)
	VALUES (
		'9789113084813', 'Hypnotisören', 'Svenska', '2009-12-01',
		(SELECT TOP 1 AuthorID FROM Authors WHERE LastName = 'Kepler'),
		(SELECT TOP 1 GenreID FROM Genres WHERE GenreName = 'Deckare'),
		(SELECT TOP 1 PublisherID FROM Publishers WHERE PublisherName = 'Norstedts')
	);
END

IF NOT EXISTS (SELECT 1 FROM Books WHERE ISBN = '9789113087341')
BEGIN
	INSERT INTO Books (ISBN, Title, Language, PublicationDate, AuthorID, GenreID, PublisherID)
	VALUES (
		'9789113087341', 'Nils Holgersson', 'Svenska', '1906-10-21',
		(SELECT TOP 1 AuthorID FROM Authors WHERE LastName = 'Selma'),
		(SELECT TOP 1 GenreID FROM Genres WHERE GenreName = 'Barnbok'),
		(SELECT TOP 1 PublisherID FROM Publishers WHERE PublisherName = 'Storyside')
	);
END

-- Lägg till lån
-- Deklareras som en  variabel av typen INT, hämtar bokens och medlemmens ID med hjälp av ISBN och Email eftersom dem är unika
-- Kontrollera så att medlemmen och boken finns samt att boken inte är utlånad
-- Om allt stämmer går lånet igenom, om inte så görs inget alls
DECLARE @BookHypnotisören INT = (SELECT TOP 1 BookID FROM Books Where ISBN = '9789113084813');
DECLARE @MemberAnton INT = (SELECT TOP 1 MemberID FROM Members Where Email = 'Anton@gmail.com');

IF @MemberAnton IS NOT NULL AND @BookHypnotisören IS NOT NULL
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Loans WHERE BookID = @BookHypnotisören AND ReturnDate IS NULL)
	BEGIN
		INSERT INTO Loans (BookID, MemberID, LoanDate, DueDate, ReturnDate)
		VALUES (@BookHypnotisören, @MemberAnton, GETDATE(), DATEADD(day, 30, GETDATE()), NULL);
	END
END
GO