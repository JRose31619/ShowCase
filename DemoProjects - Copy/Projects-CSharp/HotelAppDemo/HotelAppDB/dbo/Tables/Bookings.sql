CREATE TABLE [dbo].[Bookings]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [RoomId] INT NOT NULL, 
    [GuestsId] INT NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL, 
    [CheckedIn] BIT NOT NULL DEFAULT 0, 
    [TotalCost] MONEY NOT NULL, 
    CONSTRAINT [FK_Bookings_Rooms] FOREIGN KEY ([RoomId]) REFERENCES Rooms(Id), 
    CONSTRAINT [FK_Bookings_Guests] FOREIGN KEY ([GuestsId]) REFERENCES Guests(Id) 
)
