CREATE TABLE [dbo].[PeopleAddress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] INT NOT NULL, 
    [AddressId] INT NOT NULL, 
    CONSTRAINT [FK_PeopleAddress_Employees] FOREIGN KEY ([PersonId]) REFERENCES [Employees]([Id]), 
    CONSTRAINT [FK_PeopleAddress_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id])
)
