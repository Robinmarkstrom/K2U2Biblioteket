USE LibraryDb;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

DECLARE @MemberID INT = 3;
DECLARE @BookID INT = 4;


BEGIN TRANSACTION;

BEGIN TRY

    SELECT BookID
    FROM Loans WITH (UPDLOCK, HOLDLOCK)
    WHERE BookID = @BookID AND ReturnDate IS NULL;


    IF EXISTS (
        SELECT 1
        FROM Loans
        WHERE BookID = @BookID AND ReturnDate IS NULL
    )
    BEGIN
        PRINT 'Boken är redan utlånad';
        ROLLBACK TRANSACTION;
        RETURN;
    END

    INSERT INTO Loans (BookID, MemberID, LoanDate, DueDate, ReturnDate)
    VALUES (
        @BookID,
        @MemberID,
        GETDATE(),
        DATEADD(day, 30, GETDATE()),
        NULL
    );

    COMMIT TRANSACTION;
    PRINT 'Lån registrerat';

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT 'FEL: ' + ERROR_MESSAGE();

END CATCH;
