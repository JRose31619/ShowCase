using EDBLIbrary.Models;
using EDBWPF.Windows;
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
	/// Interaction logic for ReadWindow.xaml
	/// </summary>
	public partial class ReadWindow : Window
    {
		private readonly string _db;

		public ReadWindow(string db)
        {
            InitializeComponent();
			_db = db;

			currentDataBase.Text = $"Current DataBase Is {_db}";
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();

			window.Show();

			Close();
        }

		private void nextButton_Click(object sender, RoutedEventArgs e)
		{
			AddressSearch window = new AddressSearch(_db);

			window.Show();

			Close();
        }

		private void submitButton_Click(object sender, RoutedEventArgs e)
		{
			UIActions action = new UIActions();

			if (InputValidation.ValidateSubmittedInfo(idEntry.Text) &&
				InputValidation.ValidateExistingEmployee(idEntry.Text,_db))
			{
				FullInfoModel employeeData = action.RetrieveEmployeeInfo(idEntry.Text, _db);

				nameText.Text = $"{employeeData.BasicInfo.FirstName} {employeeData.BasicInfo.LastName}";

				if (employeeData.EmployerInfo == null)
				{
					titleText.Text = "N/A";
					accessCodeText.Text = "N/A";
				}
				else if (employeeData.EmployerInfo != null)
				{
					titleText.Text = employeeData.EmployerInfo.EmployerTitle;
					accessCodeText.Text = employeeData.EmployerInfo.AccessCode;
				}
			}
		}
	}
}
