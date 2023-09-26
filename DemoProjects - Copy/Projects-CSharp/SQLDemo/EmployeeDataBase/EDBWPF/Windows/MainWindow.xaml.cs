using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
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
using EDBWPF.Windows;

namespace EDBWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public string db;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void sqlServerButton_Click(object sender, RoutedEventArgs e)
		{
			db = "SqlServer";
			currentDataBase.Text = $"Current DateBase Is {db}";
        }

		private void sqliteButton_Click(object sender, RoutedEventArgs e)
		{
			db = "Sqlite";
			currentDataBase.Text = $"Current DateBase Is {db}";
		}

		private void mySqlButton_Click(object sender, RoutedEventArgs e)
		{
			db = "MySql";
			currentDataBase.Text = $"Current DateBase Is {db}";
		}

		private void ActionButton_Click(object sender, RoutedEventArgs e)
		{
			if (db == null) 
			{
				MessageBox.Show("You must select a database before selecting action");
			}
			else if (e.Source == createButton && db != null)
			{
				CreateWindow window = new CreateWindow(db);
				window.Show();
				Close();
			}
			else if (e.Source == addButton && db != null)
			{
				AddWindow window = new AddWindow(db);
				window.Show();
				Close();
			}
			else if (e.Source == readButton && db != null) 
			{
				ReadWindow window = new ReadWindow(db);
				window.Show();
				Close();
			}
			else if (e.Source == updateButton && db != null)
			{
				UpdateInfo window = new UpdateInfo(db);
				window.Show();
				Close();
			}
			else if(e.Source == deleteButton && db != null) 
			{
				DeleteWindow window = new DeleteWindow(db);
				window.Show();
				Close();
			}
		}
	}
}
