CREATE PROCEDURE [dbo].[spBookings_GetAllBookings]
AS
begin
	select [b].[Id], [b].[RoomId], [b].[GuestsId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalCost],
		[g].[FirstName], [g].[LastName],
		[r].[RoomNumber], [r].[RoomTypeId], 
		[rt].[Title], [rt].[Description], [rt].[Price]
	from dbo.Bookings b
	inner join dbo.Guests g on g.Id = b.GuestsId
	inner join dbo.Rooms r on r.Id = b.RoomId
	inner join dbo.RoomTypes rt on rt.Id = r.RoomTypeId
end
