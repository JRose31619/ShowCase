using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.DataModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelApp.DeskTop.Windows
{
    /// <summary>
    /// Interaction logic for DeleteConfirmation.xaml
    /// </summary>
    public partial class DeleteConfirmation : Window
    {
		private readonly IDataBaseData _db;
		private BookingModel _data = null;

		public DeleteConfirmation(IDataBaseData db)
        {
			InitializeComponent();
			_db = db;
        }

		public void PopulateInformation(BookingModel data)
		{
			_data = data;
			firstNameText.Text = data.FirstName;
			lastNameText.Text = data.LastName;
			titleText.Text = data.Title;
			roomNumberText.Text = data.RoomNumber;
			totalCostText.Text = String.Format("{0:C}", _data.TotalCost);
			startDateText.Text = data.StartDate.ToString();
			endDateText.Text = data.EndDate.ToString();
		}

		private void DeleteUser_Click(object sender, RoutedEventArgs e)
		{
			_db.DeleteBooking(_data.FirstName,_data.LastName,_data.StartDate,_data.EndDate);

			MessageBox.Show("Booking is deleted from records.");

			Close();
		}
	}
}
