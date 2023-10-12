using EDBLIbrary.Models;
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

namespace EDBWPF
{
	/// <summary>
	/// Interaction logic for DeleteWindow.xaml
	/// </summary>
	public partial class DeleteWindow : Window
    {
		private readonly string _db;

		public DeleteWindow(string dataBaseChoice)
        {
            InitializeComponent();
			_db = dataBaseChoice;

			currentDataBase.Text = $"Current DataBase Is {_db}";
		}

		private void deleteAddressButton_Click(object sender, RoutedEventArgs e)
		{
			AddressModel submittedAddress = new AddressModel() 
			{
				StreetAddress = streetAddressEntry.Text,
				City = cityEntry.Text,
				State = stateEntry.Text,
				ZipCode = zipCodeEntry.Text,
			};

			UIActions action = new UIActions();

			if (InputValidation.ValidateSubmittedInfo(submittedAddress) &&
				InputValidation.ValidateExistingEmployee(idEntry.Text,_db) &&
				InputValidation.ValidateExistingAddress(idEntry.Text,_db,submittedAddress))
			{
				submittedAddress.Id = action.RetrieveAddressKey(idEntry.Text, _db, submittedAddress);

				action.DeleteAddress(_db, idEntry.Text, submittedAddress);
			}
        }

		private void deleteEverythingButton_Click(object sender, RoutedEventArgs e)
		{
			UIActions action = new UIActions();

			if (InputValidation.ValidateSubmittedInfo(idEntry.Text) &&
				InputValidation.ValidateExistingEmployee(idEntry.Text,_db))
			{
				int employerId = action.RetreiveEmployerKey(idEntry.Text, _db);

				action.DeleteAllInfo(_db,int.Parse(idEntry.Text),employerId);
			}
        }

		private void deleteEmployerButton_Click(object sender, RoutedEventArgs e)
		{
			UIActions action = new UIActions();

			if (InputValidation.ValidateExistingEmployee(idEntry.Text,_db)&&
				InputValidation.ValidateExistingEmployerInfo(idEntry.Text,_db))
			{
				int employerId = action.RetreiveEmployerKey(idEntry.Text, _db);

				action.DeleteEmployerInfo(int.Parse(idEntry.Text),employerId,_db);
			}
        }

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();

			window.Show();

			Close();
		}
	}
}
