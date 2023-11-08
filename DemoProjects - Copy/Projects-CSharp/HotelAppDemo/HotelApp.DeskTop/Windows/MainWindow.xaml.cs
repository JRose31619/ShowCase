using HotelApp.DeskTop.Windows;
using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.DataBaseAccess;
using HotelAppClassLibrary.DataModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelApp.DeskTop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IDataBaseData _db;

		public MainWindow(IDataBaseData db)
		{
			InitializeComponent();
			_db = db;
		}

		private void searchForGuest_Click(object sender, RoutedEventArgs e)
		{
			List<BookingModel> bookings = _db.SearchBookings(lastNameText.Text);
			resultsList.ItemsSource = bookings;
		}

		private void CheckInButton_Click(object sender, RoutedEventArgs e)
		{
			var checkInForm = App.serviceProvider.GetService<CheckInForm>();
			var model = (BookingModel)((Button)e.Source).DataContext;

			checkInForm.PopulateCheckInfo(model);
			checkInForm.Show();
		}

		private void getBookingsWindow_Click(object sender, RoutedEventArgs e)
		{
			DeleteBookings window = new DeleteBookings(_db);

			window.Show();

			Close();
        }
    }
}
