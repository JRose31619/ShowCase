using HotelAppClassLibrary.DataModels;
using System;
using System.Collections.Generic;

namespace HotelAppClassLibrary.Data
{
	public interface IDataBaseData
	{
		void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId);
		void CheckGuestIn(int bookingId);
		List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
		RoomTypeModel GetRoomTypeById(int id);
		List<BookingModel> SearchBookings(string lastName);
		List<BookingModel> GetAllBookings();
		void DeleteBooking(string firstName, string lastName, DateTime startDate, DateTime endDate);
	}
}