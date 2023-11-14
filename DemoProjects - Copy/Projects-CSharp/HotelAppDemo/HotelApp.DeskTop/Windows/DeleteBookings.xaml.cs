using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.DataModels;
using Microsoft.Extensions.DependencyInjection;
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
	/// Interaction logic for DeleteBookings.xaml
	/// </summary>
	public partial class DeleteBookings : Window
	{
		private readonly IDataBaseData _db;

		public DeleteBookings(IDataBaseData db)
		{
			InitializeComponent();
			_db = db;
		}

		private void searchForBookings_Click(object sender, RoutedEventArgs e)
		{
			List<BookingModel> bookings = _db.GetAllBookings();
			resultsList.ItemsSource = bookings;
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow(_db);

			window.Show();

			Close();
        }

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			var deleteConfirmation = App.serviceProvider.GetService<DeleteConfirmation>();
			var model = (BookingModel)((Button)e.Source).DataContext;

			deleteConfirmation.PopulateInformation(model);
			deleteConfirmation.Show();
		}
	}
}
