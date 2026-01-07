USE LibraryDb;
GO

-- Index på books
-- Snabb lookup per book eftersom ISBN är unik
-- Används vid sökningar av specifika böcker
CREATE UNIQUE INDEX IX_Books_ISBN
ON Books(ISBN);

-- Eftersom FK används ofta i JOINS så är det bra att ha index på dessa
CREATE NONCLUSTERED INDEX IX_Books_AuthorID
ON Books(AuthorID);

CREATE NONCLUSTERED INDEX IX_Books_GenreID
ON Books(GenreID);

CREATE NONCLUSTERED INDEX IX_Books_PublisherID
ON Books(PublisherID);

-- Index på members
-- Members hittas ofta av e-post eftersom den är unik
CREATE UNIQUE INDEX IX_Members_Email
ON Members(Email);

-- Index på Loans
-- Görs många sökningar om boken är utlånad och då är alltid retundate med
CREATE NONCLUSTERED INDEX IX_Loans_BookID_ReturnDate
ON Loans(BookID, ReturnDate);

-- Visar alla lån per medlem då filtrerar man ofta på returndate
CREATE NONCLUSTERED INDEX IX_Loans_MemberID_ReturnDate
ON Loans(MemberID, ReturnDate);

