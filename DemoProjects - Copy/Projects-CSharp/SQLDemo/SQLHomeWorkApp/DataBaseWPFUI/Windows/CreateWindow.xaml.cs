using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
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

namespace DataBaseWPFUI
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

			if (InputValidation.ValidateNewUserEntry(userInput))
			{
				UIActions action = new UIActions();

				action.InsertNewUser(_db, userInput);

				MessageBox.Show("New entry entered successfully!");
			}
			else
			{
				MessageBox.Show("New user entry failed to enter");
			}
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			Close();
		}
	}
}
