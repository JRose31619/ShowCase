using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
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

namespace DataBaseWPFUI
{
	/// <summary>
	/// Interaction logic for AddWindow.xaml
	/// </summary>
	public partial class AddWindow : Window
	{
		private readonly string _db;

		public AddWindow(string db)
		{
			InitializeComponent();
			_db = db;

            currentDataBase.Text = $"Current DateBase Is {_db}";
		}

		private void submitAddressButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> submittedInfo = new List<string>
			{
				firstNameEntry.Text,
				lastNameEntry.Text,
				streetAddressEntry.Text,
				cityEntry.Text,
				stateEntry.Text,
				zipCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(submittedInfo) &&
				InputValidation.ValidateExistingEmployee(firstNameEntry.Text,
											 lastNameEntry.Text,
											 idEntry.Text,
											 _db)) 
			{

				EmployeeAddressModel newAddress = new EmployeeAddressModel()
				{
					FirstName = firstNameEntry.Text,
					LastName = lastNameEntry.Text,
					StreetAddress = streetAddressEntry.Text,
					City = cityEntry.Text,
					State = stateEntry.Text,
					ZipCode = zipCodeEntry.Text,
				};

				UIActions action = new UIActions();

				action.InsertNewAddress(_db, newAddress);
			}
        }

		private void submitEmployerButton_Click(object sender, RoutedEventArgs e)
		{

			List<string> submittedInfo = new List<string>()
			{
				firstNameEntry.Text,
				lastNameEntry.Text,
				employerTitleEntry.Text,
				accessCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(submittedInfo) &&
				InputValidation.ValidateExistingEmployee(firstNameEntry.Text,
											 lastNameEntry.Text,
											 idEntry.Text,
											 _db) &&
				InputValidation.ValidateExistingEmployerInfo(idEntry.Text,_db) == false)
			{
				FullInfoModel newEmployerInfo = new FullInfoModel();

				newEmployerInfo.BasicInfo = new BasicInfoModel() 
				{
					FirstName = firstNameEntry.Text,
					LastName = lastNameEntry.Text,
				};

				newEmployerInfo.EmployerInfo = new EmployerModel()
				{
					EmployerTitle = employerTitleEntry.Text,
					AccessCode = accessCodeEntry.Text,
				};

				UIActions action = new UIActions();

				action.InsertNewEmployerInfo(_db,newEmployerInfo);
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
