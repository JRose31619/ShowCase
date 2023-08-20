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

namespace DataBaseWPFUI.Windows
{
	/// <summary>
	/// Interaction logic for UpdateInfo.xaml
	/// </summary>
	public partial class UpdateInfo : Window
	{
		private readonly string _db;

		public UpdateInfo(string db)
		{
			InitializeComponent();
			_db = db;

			currentDataBase.Text = $"Current DataBase is {_db}";
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();

			window.Show();

			Close();
		}

		private void nextButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateAddress window = new UpdateAddress(_db);

			window.Show();

			Close();
		}

		// Remeber to fix this, has same problem as employer button
		private void submitNameButton_Click(object sender, RoutedEventArgs e)
		{
			string id = idEntry.Text;

			UIActions action = new UIActions();

			BasicInfoModel newName = new BasicInfoModel()
			{
				FirstName = firstNameEntry.Text,
				LastName = lastNameEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(newName) && 
				InputValidation.ValidateExistingEmployee(id, _db))
			{
				newName.Id = action.RetrievePrimaryKey(id, _db);

				action.UpdateName(_db, newName);
			}
		}

		private void submitEmployerButton_Click(object sender, RoutedEventArgs e)
		{
			string id = idEntry.Text;
			UIActions action = new UIActions();

			EmployerModel newEmployerInfo = new EmployerModel()
			{
				EmployerTitle = newTitleEntry.Text,
				AccessCode = newAccessCodeEntry.Text,
			};

			if (InputValidation.ValidateSubmittedInfo(newEmployerInfo) &&
				InputValidation.ValidateExistingEmployerInfo(id,_db))
			{
				newEmployerInfo.Id = action.RetreiveEmployerKey(id, _db);

				action.UpdateEmployerInfo(_db, newEmployerInfo);
			}


		}
	}
}
