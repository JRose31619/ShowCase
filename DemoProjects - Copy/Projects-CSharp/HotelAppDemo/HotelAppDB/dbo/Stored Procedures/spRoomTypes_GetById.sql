CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
	@id int
AS
begin  
	select [Id], [Title], [Description], [Price]
	from dbo.RoomTypes
	where Id = @id
end

