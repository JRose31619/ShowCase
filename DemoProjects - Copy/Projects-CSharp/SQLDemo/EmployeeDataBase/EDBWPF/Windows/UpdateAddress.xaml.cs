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

namespace EDBWPF.Windows
{
	/// <summary>
	/// Interaction logic for UpdateAddress.xaml
	/// </summary>
	public partial class UpdateAddress : Window
	{
		private readonly string _dataBaseChoice;

		public UpdateAddress(string dataBaseChoice)
		{
			InitializeComponent();
			_dataBaseChoice = dataBaseChoice;

			currentDataBase.Text = $"Current DataBase Is {_dataBaseChoice}";
		}

		private void submitAddressesButton_Click(object sender, RoutedEventArgs e)
		{
			List<TextBox> textBoxes = new List<TextBox>() 
			{
				newStreetAddressEntry,
				newCityEntry,
				newStateEntry,
				newZipCodeEntry,
				previousStreetAddressEntry,
				previousCityEntry,
				previousStateEntry,
				previousZipCodeEntry,
			};

			List<string> submittedInfo = new List<string>() 
			{
				newStreetAddressEntry.Text,
				newCityEntry.Text,
				newStateEntry.Text,
				newZipCodeEntry.Text,
				previousStreetAddressEntry.Text,
				previousCityEntry.Text,
				previousStateEntry.Text,
				previousZipCodeEntry.Text,
			};

			AddressModel previousAddress = new AddressModel()
			{
				StreetAddress = previousStreetAddressEntry.Text,
				City = previousCityEntry.Text,
				State = newStateEntry.Text,
				ZipCode = previousZipCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(submittedInfo) && 
				InputValidation.ValidateExistingAddress(idEntry.Text, _dataBaseChoice, previousAddress))
			{
				UIActions action = new UIActions();

				AddressModel newAddress = new AddressModel()
				{
					Id = action.RetrieveAddressKey(idEntry.Text, _dataBaseChoice, previousAddress),
					StreetAddress = newStreetAddressEntry.Text,
					City = newCityEntry.Text,
					State = newStateEntry.Text,
					ZipCode = newZipCodeEntry.Text,
				};

				action.UpdateAddress(_dataBaseChoice, newAddress);

				CleanUp.ClearTextBoxes(textBoxes);
			}
        }

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateInfo window = new UpdateInfo(_dataBaseChoice);

			window.Show();

			Close();
		}

		private void homeButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();

			window.Show();

			Close();
		}
	}
}
