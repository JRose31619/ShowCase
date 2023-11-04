CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@id int
AS
begin
	set nocount on

	update dbo.Bookings
	set CheckedIn = 1
	Where Id = @id
end
