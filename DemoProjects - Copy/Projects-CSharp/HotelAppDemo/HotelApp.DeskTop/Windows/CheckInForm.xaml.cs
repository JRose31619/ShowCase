using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.DataModels;
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

namespace HotelApp.DeskTop
{
    /// <summary>
    /// Interaction logic for CheckInForm.xaml
    /// </summary>

    public partial class CheckInForm : Window
    {
		private readonly IDataBaseData _db;
		private BookingModel _data = null;
		public CheckInForm(IDataBaseData db)
        {
            InitializeComponent();
			_db = db;
		}

        public void PopulateCheckInfo(BookingModel data) 
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

		private void checkInUser_Click(object sender, RoutedEventArgs e)
		{
            _db.CheckGuestIn(_data.Id);

            MessageBox.Show("Guest is now checked in!");

            Close();
		}
	}
}
