# READ ME

## Summary
The system is built on SQL Server and accessed through a C# console application using Entity Framework.

This Project implements a library management system that handles: 
- Book records
- Library members
- Active and returned loans

## The goal of the project is to demonstrate
- Relational database modeling
- Data integrity and referential constraints
- Transaction-safe loan handling
- Indexing and optimization techniques

## How to Run
### 1. Database Setup
1. Open SQL Server Management Studio (SSMS)
2. Execute the provided SQL script to create the database LibraryDb, tables, and indexes.
3. (Optional) Run the insert script to populate the database with test data.

### 2. Application Setup
1. Open the solution in Visual Studio
2. Open appsettings.json and ensure the Connection String matches your local SQL Server instance.
3. Login credentials for the console is in the 'AuthenticatorService.cs' file.
4. Run the application (Ctrl + F5).
5. Follow the console promts.

## Entity-Relationship Diagram (ERD)
The diagram below illustrates the database structure, entity relationships, and cardinality.

<img width="4491" height="2445" alt="EntityRelationshipDiagram" src="https://github.com/user-attachments/assets/a412b6e0-e8c3-45bf-987d-504b5b860f2e" />

## Reflections
The database schema is designed according to Third Normal Form (3NF) to ensure data consistency and reduce redundancy.
Entities like Authors, Genres, and Publishers are separated into their own tables. This prevents data duplication (repeating the author's name for every book) and ensures that updates (a publisher changing their name) only need to happen in one place.
UNIQUE constraints are applied to ISBN (Books) and Email (Members) to prevent duplicate records. NOT NULL constraints ensure that essential data is always present.
Foreign Keys strictly enforce relationships. It is impossible to assign a book to a non-existent author or register a loan for a non-existent member.
The stored procedure for loans utilizes WITH (UPDLOCK, HOLDLOCK). This locks the specific row in the Loans table during the read process. If a second transaction tries to read the same book's status before the first transaction is committed, it is forced to wait. This guarantees that the system never lends an already borrowed book.
Unique indexes on Books.ISBN and Members.Email optimize the most common lookup queries.
Indexes on Foreign Keys (AuthorID, GenreID) improve the performance of JOIN operations when generating reports or listing books with their related details.

## Execution Plan Example
### Index show case query
<img width="439" height="197" alt="Query" src="https://github.com/user-attachments/assets/fedbe327-dbdf-44ea-90d1-1d1ec08f58dc" />

### Index show case execution plan
<img width="1482" height="729" alt="Execution_Plan" src="https://github.com/user-attachments/assets/8b30e74e-f865-407f-b1c8-225b33f475bf" />

