using EDBLIbrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
	/// Interaction logic for CreateWindow.xaml
	/// </summary>
	public partial class CreateWindow : Window
    {
		private readonly string _db;

		public CreateWindow(string db)
        {
            InitializeComponent();
			_db = db;

            currentDataBase.Text = $"Current DateBase Is {_db}";
		}

		private void submitButton_Click(object sender, RoutedEventArgs e)
		{
			FullInfoModel userInput = new FullInfoModel();

			List<TextBox> textBoxes = new List<TextBox>() 
			{
				streetAddressEntry,
				cityEntry,
				stateEntry,
				zipCodeEntry,
				firstNameEntry,
				lastNameEntry,
				managerTitleEntry,
				accessCodeEntry
			};

			userInput.BasicInfo = new BasicInfoModel()
			{
				FirstName = firstNameEntry.Text,
				LastName = lastNameEntry.Text,
			};

			AddressModel addressInfo = new AddressModel()
			{
				StreetAddress = streetAddressEntry.Text,
				City = cityEntry.Text,
				State = stateEntry.Text,
				ZipCode = zipCodeEntry.Text,
			};

			userInput.Addresses.Add(addressInfo);

			userInput.EmployerInfo = new EmployerModel()
			{
				EmployerTitle = managerTitleEntry.Text,
				AccessCode = accessCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(userInput) &&
				InputValidation.ValidateNewUserEntry(userInput))
			{
				UIActions action = new UIActions();

				action.InsertNewUser(_db, userInput);

				MessageBox.Show("New entry entered successfully! User Id is" );
			}
			else
			{
				MessageBox.Show("New user entry failed to enter");
			}

			CleanUp.ClearTextBoxes(textBoxes);
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			Close();
		}
	}
}
