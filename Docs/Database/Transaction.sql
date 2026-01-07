USE LibraryDb;
GO

SET XACT_ABORT ON; -- Rullar tillbaka transaktionen vid fel
SET NOCOUNT ON; -- Mindre logg 

DECLARE @MemberID INT = 3; -- Medlem som lånar (Tobias Dewall)
DECLARE @BookID INT = 2; -- Bok som ska lånas (Harry Potter)

BEGIN TRANSACTION; -- Startar transaktionen (allt som sker här inne måste lyckas)

BEGIN TRY
	SELECT 1
	FROM Loans WITH (UPDLOCK, HOLDLOCK) -- UPDLOCK vill uppdatera data så ingen får ändra något men kan läsas. HOLDLOCK håller låset till transaktionen är klar
	WHERE BookID = @BookID AND ReturnDate IS NULL; -- Kollar om boken redan är utlånad

	IF @@ROWCOUNT > 0 -- Returnerar antal rader som SELECT 1 hittade
	BEGIN
		PRINT 'FEL: Boken är redan utlånad!'; -- Om inga rader returnerades så är boken redan utlånad
		ROLLBACK TRANSACTION; -- Återställer allt som gjorts i transaktionen
		RETURN;
	END

	INSERT INTO Loans (BookID, MemberID, LoanDate, DueDate, ReturnDate) -- Boken finns att låna 
	VALUES (
		@BookID, -- Vilken bok lånas
		@MemberID, -- Vem som lånar
		GETDATE(), -- Datum när lånet startar (dagens datum)
		DATEADD(day, 30, GETDATE()), -- Förfallodatum (30 dagar)
		NULL -- ReturnDate NULL = boken är fortfarande utlånad
	);

	COMMIT TRANSACTION; -- Bekräftar lånet och gör det permanent
	PRINT 'GODKÄNT: Lånet har registrerats!';

END TRY
BEGIN CATCH -- Om något fel uppstår 

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION; -- Säkerställer att transaktionen rullas tillbaka

	PRINT 'FEL: Transaktionen avbröts!';
	PRINT ERROR_MESSAGE(); -- Skriver ut SQL-felmeddelandet

END CATCH;