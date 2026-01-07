-- Använder rätt databas
USE LibraryDb;
GO

-- Visa alla böcker
SELECT
	b.BookID, -- Bokens ID
	b.ISBN, -- ISBN-nummer
	b.Title, -- Boktitel
	a.FirstName + ' ' + a.LastName AS Author, -- Författarens fullständiga namn
	g.GenreName, -- Bokgenre
	p.PublisherName, -- Förlag
	b.PublicationDate -- Publiceringsdatum
FROM Books b
JOIN Authors a ON b.AuthorID = a.AuthorID -- Koppla boken till författaren
JOIN Genres g ON b.GenreID = g.GenreID -- Koppla boken till genren
JOIN Publishers p ON b.PublisherID = p.PublisherID; -- Koppla boken till förlaget

-- Sök efter specifik bok utifrån titel
-- Titeln på boken måste innehåll 'Harry'
SELECT * 
FROM Books 
WHERE Title LIKE '%Harry%';

-- Visa alla medlemmar
-- Hämtar all information om medlemmar
-- Behövs ingen join för all information finns i members tabellen
SELECT
	MemberID,
	FirstName,
	LastName,
	Email,
	PhoneNumber,
	RegistrationDate
FROM Members;

-- Visa alla böcker som är utlånade
SELECT
	l.LoanID, -- Låne-ID
	b.title AS Bok, -- Boktitel
	m.FirstName + ' ' + m.LastName AS Låntagare, -- Låntagarens fullständiga namn
	l.LoanDate AS Lånedatum, -- Datum när boken lånades
	l.DueDate AS Förfallodatum -- Datum då boken ska återlämnas
FROM Loans l
JOIN Books b ON l.BookID = b.BookID -- Koppla lånet till boken
JOIN Members m ON l.MemberID = m.MemberID -- Koppla lånet till medlemmen
WHERE l.ReturnDate IS NULL; -- Endast pågående lån

-- Registra lån av bok
DECLARE @MemberID INT = 2; -- Medlem som ska låna
DECLARE @BookID INT = 3; -- Bok som ska lånas

IF EXISTS (SELECT 1 FROM Loans WHERE BookID = @BookID AND ReturnDate IS NULL) -- Kontroll om boken är utlånad
BEGIN
	PRINT 'FEL: Denna bok är tyvärr redan utlånad!'; -- Boken är utlånad

	SELECT
		m.FirstName + ' ' + m.LastName AS Låntagare, -- Låntagarens fullständiga namn
		l.DueDate  -- Datum då boken ska återlämnas 
	FROM Loans l
	JOIN Members m ON l.MemberID = m.MemberID -- Koppla lånet till medlemmen 
	WHERE l.BookID = @BookID AND l.ReturnDate IS NULL; -- Hämtar post från ett specifikt bok-id som inte återlämnats än

END
ELSE -- Om boken inte är utlånad då registreras ett nytt lån
BEGIN
	INSERT INTO Loans (BookID, MemberID, LoanDate, DueDate, ReturnDate)
	VALUES (
		@BookID, -- Specifikt bok-id som ska lånas
		@MemberID, -- Specifikt medlem-id som ska låna
		GETDATE(), -- Lånedag (idag) 
		DATEADD(day, 30, GETDATE()), -- Återlämnas efter 30 dagar
		NULL -- Ej återlämnad än
	);

	PRINT 'Lånet har registrerats!'; -- Lån registrerat

	SELECT TOP 1 * FROM Loans ORDER BY LoanID DESC; -- Visar de senaste lånen som registrerats
END

-- Lämna tillbaka bok
DECLARE @ReturnBookID INT = 3; -- Specifik bok som ska återlämnas
DECLARE @LoanID INT; -- Variabel för att spara låne-ID
SELECT @LoanID = LoanID
FROM Loans
WHERE BookID = @ReturnBookID AND ReturnDate IS NULL;

IF @LoanID IS NOT NULL -- Om boken inte är återlämnad så görs den nu
BEGIN
	UPDATE Loans -- Uppdatera lånet
	SET ReturnDate = GETDATE() -- Sätter återlämningsdagen till dagens datum
	WHERE LoanID = @LoanID; 

	PRINT 'Boken är nu återlämnad!'; -- Återregistreingen av boken är gjord
END
ELSE
BEGIN
	PRINT 
		'FEL: Kunde inte hitta något pågående lån av denna bok!'; -- Finns inget lån på just denna specifika bok
END