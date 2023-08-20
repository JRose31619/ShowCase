CREATE TABLE [dbo].[PeopleEmployerLink]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] INT NOT NULL, 
    [EmployerTId] INT NOT NULL
)
