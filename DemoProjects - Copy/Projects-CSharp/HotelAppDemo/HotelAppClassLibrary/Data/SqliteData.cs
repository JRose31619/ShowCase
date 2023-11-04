using HotelAppClassLibrary.DataBaseAccess;
using HotelAppClassLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppClassLibrary.Data
{
	public class SqliteData : IDataBaseData
	{
		private readonly ISqliteDataAccess _db;
		private const string connectionStringName = "Sqlite";

		public SqliteData(ISqliteDataAccess db)
        {
			_db = db;
		}
        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
		{

			string sql = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName";
			int results =_db.LoadData<dynamic,dynamic>(sql, new { firstName,lastName}, connectionStringName).Count();

			if (results == 0)
			{
				sql = @"insert into Guests (FirstName, LastName)
						values (@firstName, @lastName);";

				_db.SaveData(sql, new { firstName, lastName }, connectionStringName);
			}

			sql = @"select [Id], [FirstName], [LastName]
						  from Guests
						  where FirstName = @firstName and LastName = @lastName LIMIT 1;";

			GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
											new { firstName, lastName },
											connectionStringName).First();

			RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id = @Id",
																 new { Id = roomTypeId },
																 connectionStringName).First();

			TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

			sql = @"select r.*
					from Rooms r
					inner join RoomTypes t on t.Id = r.RoomTypeId
					where r.RoomTypeId = @roomTypeId
					and r.Id not in (
					select b.RoomId
					from Bookings b
					where(@startDate < b.StartDate and @endDate > b.EndDate)
						or (b.StartDate <= @endDate and @endDate < b.EndDate)
						or (b.StartDate <= @startDate and @startDate < b.EndDate)
					);";

			List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
																	 new { startDate, endDate, roomTypeId },
																	 connectionStringName);

			sql = @"insert into Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
				    values (@roomId, @guestId, @startDate, @endDate, @totalCost);";

			_db.SaveData(sql, 
						 new
						{
							roomId = availableRooms.First().Id,
							guestId = guest.Id,
							startDate = startDate,
							endDate = endDate,
							totalCost = timeStaying.Days * roomType.Price
						},
						connectionStringName);

		}

		public void CheckGuestIn(int bookingId)
		{
			string sql = @"	update Bookings
							set CheckedIn = 1
							Where Id = @id";

			_db.SaveData(sql, new { id = bookingId}, connectionStringName);
		}
		public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
		{
			string sql = @"	select t.Id, t.Title, t.Description, t.Price
							from Rooms r
							inner join RoomTypes t on t.Id = r.RoomTypeId
							where r.Id not in (
							select b.RoomId
							from Bookings b
							where(@startDate < b.StartDate and @endDate > b.EndDate)
							or(b.StartDate <= @endDate and @endDate < b.EndDate)
							or(b.StartDate <= @startDate and @startDate  < b.EndDate)
							)

							group by t.Id,t.Title,t.Description,t.Price;";

			var output = _db.LoadData<RoomTypeModel, dynamic>
				   (sql,new {startDate, endDate }, connectionStringName);

			output.ForEach(x => x.Price = x.Price / 100);

			return output;
		}

		public RoomTypeModel GetRoomTypeById(int id)
		{
			string sql = @"Select [Id], [Title], [Description], [Price]
							from RoomTypes
							where Id = @id";

			return _db.LoadData<RoomTypeModel, dynamic>(sql,
											   new { id },
											   connectionStringName).FirstOrDefault();
		}

		// Left here on 11/3/2023
		// Just finished working the search bookings
		// method. have to finish the app. Put it in repo
		// and think of other small things to add to flesh
		// out the app
		public List<BookingModel> SearchBookings(string lastName)
		{
			string sql = @"	select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate],
							[b].[EndDate], [b].[CheckedIn], [b].[TotalCost],
							[g].[FirstName], [g].[LastName],
							[r].[RoomNumber], [r].[RoomTypeId], 
							[rt].[Title], [rt].[Description], [rt].[Price]
						from Bookings b
						inner join Guests g on g.Id = b.GuestId
						inner join Rooms r on r.Id = b.RoomId
						inner join RoomTypes rt on rt.Id = r.RoomTypeId
						where b.StartDate = @startDate and g.LastName = @lastName";

			var output = _db.LoadData<BookingModel, dynamic>(sql,
								  new { lastName, startDate = DateTime.Now.Date },
								  connectionStringName);

			output.ForEach(x =>
			{
				x.price = x.price / 100;
				x.TotalCost = x.TotalCost / 100;
			});

			return output;
		}
	}
}
