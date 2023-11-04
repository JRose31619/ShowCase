CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableRoomTypes]
	@startDate date,
	@endDate date
AS
begin 
	set nocount on;

	select t.Id, t.Title, t.Description, t.Price
	from dbo.Rooms r
	inner join dbo.RoomTypes t on t.Id = r.RoomTypeId
	where r.Id not in (
	select b.RoomId
	from dbo.Bookings b
	where(@startdate < b.StartDate and @endDate > b.EndDate)
	or (b.StartDate <= @endDate and @endDate < b.EndDate)
	or (b.StartDate <= @startDate and @endDate < b.EndDate)
	)

	group by t.Id,t.Title,t.Description,t.Price
end
