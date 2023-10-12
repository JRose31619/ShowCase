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
			List<TextBox>textBoxes = new List<TextBox>() 
			{
				streetAddressEntry,
				cityEntry,
				stateEntry,
				zipCodeEntry,
				idEntry,
			};

			List<string> submittedInfo = new List<string>
			{
				streetAddressEntry.Text,
				cityEntry.Text,
				stateEntry.Text,
				zipCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(submittedInfo) &&
				InputValidation.ValidateExistingEmployee(idEntry.Text,_db)) 
			{

				EmployeeAddressModel newAddress = new EmployeeAddressModel()
				{
					PersonId = int.Parse(idEntry.Text),
					StreetAddress = streetAddressEntry.Text,
					City = cityEntry.Text,
					State = stateEntry.Text,
					ZipCode = zipCodeEntry.Text,
				};

				UIActions action = new UIActions();

				action.InsertNewAddress(_db, newAddress);

				CleanUp.ClearTextBoxes(textBoxes);
			}
        }

		private void submitEmployerButton_Click(object sender, RoutedEventArgs e)
		{

			List<TextBox>textBoxes = new List<TextBox>() 
			{
				idEntry,
				employerTitleEntry,
				accessCodeEntry,
			};

			List<string> submittedInfo = new List<string>()
			{
				employerTitleEntry.Text,
				accessCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(submittedInfo) &&
				InputValidation.ValidateExistingEmployee(idEntry.Text,_db) &&
				InputValidation.ValidateExistingEmployerInfo(idEntry.Text,_db) == false)
			{
				FullInfoModel newEmployerInfo = new FullInfoModel();

				newEmployerInfo.EmployerInfo = new EmployerModel()
				{
					EmployerTitle = employerTitleEntry.Text,
					AccessCode = accessCodeEntry.Text,
				};

				UIActions action = new UIActions();

				action.InsertNewEmployerInfo(_db,newEmployerInfo);

				CleanUp.ClearTextBoxes(textBoxes);
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
