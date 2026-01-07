-- Vill kunna köra hela skriptet direkt
-- Skapar databasen om den inte nu redan finns
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LibraryDb')
BEGIN
	CREATE DATABASE LibraryDb;
END
GO

-- Använd den nyskapade databasen
USE LibraryDb;
GO

-- Skapa alla tabeller
-- Börjar med tabeller utan beroenden
-- Tabell: Authors
CREATE TABLE Authors (
	AuthorID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	FirstName NVARCHAR(50) NOT NULL, -- Förnamn
	LastName NVARCHAR(50) NOT NULL -- Efternamn
);

-- Tabell: Genres
CREATE TABLE Genres (
	GenreID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	GenreName NVARCHAR(50) UNIQUE NOT NULL -- Unikt genrenamn
);

-- Tabell: Publishers
CREATE TABLE Publishers (
	PublisherID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	PublisherName NVARCHAR(50) UNIQUE NOT NULL -- Unikt förlagsnamn
);

-- Tabell: Members
CREATE TABLE Members (
	MemberID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	FirstName NVARCHAR(50) NOT NULL, -- Förnamn
	LastName NVARCHAR(50) NOT NULL, -- Efternamn
	Email NVARCHAR(50) UNIQUE NOT NULL, -- Unik e-post
	PhoneNumber NVARCHAR(20) NOT NULL, -- Telefonnummer
	RegistrationDate DATE DEFAULT GETDATE() -- Registreringsdatum (dagens datum)
);

-- Tabell: Books
CREATE TABLE Books (
	BookID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	ISBN NVARCHAR(13) UNIQUE NOT NULL, -- Unikt artikelnummer till bok
	Title NVARCHAR(50) NOT NULL, -- Titel
	Language NVARCHAR(50) DEFAULT 'Svenska', -- Språk (svenska är standard)
	PublicationDate DATE NOT NULL, -- Publiceringsdatum

	-- FK
	AuthorID INT NOT NULL, -- Kopplar  till Authors
	GenreID INT NOT NULL, -- Kopplar  till Genres
	PublisherID INT NOT NULL, -- Kopplar  till Publishers

	-- Kopplingar till andra tabeller
	CONSTRAINT FK_Books_Authors FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID), -- Relation  bok till författare
	CONSTRAINT FK_Books_Genres FOREIGN KEY (GenreID) REFERENCES Genres(GenreID), -- Relation  bok till genre
	CONSTRAINT FK_Books_Publishers FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID) -- Relation  bok till förlag
);

-- Tabell: Loans 
CREATE TABLE Loans (
	LoanID INT IDENTITY PRIMARY KEY, -- Primärnyckel
	LoanDate DATE DEFAULT GETDATE(), -- Lånedatum (dagens datum)
	DueDate DATE NOT NULL, -- Förfallodatum
	ReturnDate DATE NULL, -- Återlämnad datum (NULL betyder att boken inte är återlämnad)

	-- FK
	BookID INT NOT NULL, -- Kopplar  till Books
	MemberID INT NOT NULL, -- Kopplar  till Members

	-- Kopplingar till andra tabeller
	CONSTRAINT FK_Loans_Books FOREIGN KEY (BookID) REFERENCES Books(BookID), -- Relation  lån till bok
	CONSTRAINT FK_Loans_Members FOREIGN KEY (MemberID) REFERENCES Members(MemberID) -- Relation  lån till medlem
);
GO