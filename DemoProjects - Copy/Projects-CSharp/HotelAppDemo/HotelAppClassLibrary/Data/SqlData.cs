 using HotelAppClassLibrary.DataBaseAccess;
using HotelAppClassLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppClassLibrary.Data
{
	public class SqlData : IDataBaseData
	{
		private readonly ISqlDataAccess _db;
		private const string connectionStringName = "SqlDB";
		public SqlData(ISqlDataAccess db)
		{
			_db = db;
		}
		public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
		{
			return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableRoomTypes",
							   new { startDate, endDate },
							   connectionStringName,
							   true);
		}

		public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
		{
			GuestModel guest = _db.LoadData<GuestModel, dynamic>("spGuests_Insert",
														new {firstName,lastName },
														connectionStringName,
														true).First();

			RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
																 new { Id = roomTypeId },
																 connectionStringName,
																 false).First();

			TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

			List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("spRooms_GetAvailableRooms",
																	 new { startDate, endDate, roomTypeId },
																	 connectionStringName,
																	 true);

			_db.SaveData("spBookings_insert",
												new
												{
													roomId = availableRooms.First().Id,
													guestId = guest.Id,
													startDate = startDate,
													endDate = endDate,
													totalCost = timeStaying.Days * roomType.Price
												},
												connectionStringName,
												true);
		}

		public List<BookingModel> SearchBookings(string lastName)
		{
			return _db.LoadData<BookingModel, dynamic>("spBookings_GetBookings",
											  new { lastName, currentDate = DateTime.Now.Date },
											  connectionStringName,
											  true);
		}

		public void CheckGuestIn(int id)
		{
			_db.SaveData("spBookings_CheckIn", new {id}, connectionStringName, true);
		}

		public RoomTypeModel GetRoomTypeById(int id) 
		{
			return _db.LoadData<RoomTypeModel, dynamic>("spRoomTypes_GetById", new { id }, connectionStringName, true).FirstOrDefault();
		}

		public List<BookingModel> GetAllBookings()
		{
			return _db.LoadData<BookingModel, dynamic>("spBookings_GetAllBookings",
											  new { },
											  connectionStringName,
											  true);
		}

		public void DeleteBooking(string firstName, string lastName, DateTime startDate, DateTime endDate)
		{

			string sql = "Select Id from dbo.Guests where FirstName = @firstName and LastName = @lastName";

			int guestId = _db.LoadData<BookingModel, dynamic>(sql,
													 new { firstName, lastName },
													 connectionStringName,
													 false).First().Id;

			sql = @"Delete from dbo.Bookings where GuestsId = @guestId and
					StartDate = @startDate and EndDate = @endDate";

			_db.SaveData<dynamic>(sql, new {guestId,startDate,endDate }, connectionStringName,false);
		}
	}
}
