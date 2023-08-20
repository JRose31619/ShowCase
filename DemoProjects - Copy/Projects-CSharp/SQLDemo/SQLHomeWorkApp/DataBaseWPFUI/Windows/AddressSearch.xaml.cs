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
    /// Interaction logic for AddressSearch.xaml
    /// </summary>
    public partial class AddressSearch : Window
    {
        private readonly string _db;
        public AddressSearch(string db)
        {
            InitializeComponent();

            _db = db;

			currentDataBase.Text = $"Current DataBase Is {_db}";
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
            ReadWindow window = new ReadWindow(_db);

            window.Show();

            Close();
		}

		private void homeButton_Click(object sender, RoutedEventArgs e)
		{
            MainWindow window = new MainWindow();

            window.Show();

            Close();
		}

		private void submitButton_Click(object sender, RoutedEventArgs e)
		{
            UIActions action = new UIActions();

            List<string> submittedInfo = new List<string>() 
            {
                idEntry.Text,
                addressEntry.Text,
            };

            if (InputValidation.ValidateSubmittedInfo(submittedInfo) &&
                InputValidation.ValidateExistingEmployee(idEntry.Text,_db) &&
                InputValidation.ValidateExistingAddress(idEntry.Text,_db,addressEntry.Text)) 
            {
                int selectedAddress = int.Parse(addressEntry.Text) - 1;
                FullInfoModel employeeData = action.RetrieveEmployeeInfo(idEntry.Text,_db);

                streetText.Text = employeeData.Addresses[selectedAddress].StreetAddress;
                cityText.Text = employeeData.Addresses[selectedAddress].City;
                stateText.Text = employeeData.Addresses[selectedAddress].State;
                zipCodeText.Text = employeeData.Addresses[selectedAddress].ZipCode;
            }
		}
	}
}
