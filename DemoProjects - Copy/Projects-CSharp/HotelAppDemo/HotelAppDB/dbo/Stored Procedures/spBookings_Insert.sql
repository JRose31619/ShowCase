﻿CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId int,
	@guestId int,
	@startDate date,
	@endDate date,
	@totalCost money
AS
begin
	insert into dbo.bookings(RoomId,GuestsId,StartDate,EndDate,TotalCost)
	values (@roomId,@guestId,@startDate,@endDate,@totalCost)
end
