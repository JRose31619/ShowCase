﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.DataBaseAccess;

namespace HotelApp.DeskTop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static ServiceProvider serviceProvider;
		protected override void OnStartup(StartupEventArgs e) 
		{
			base.OnStartup(e);

			var services = new ServiceCollection();
			services.AddTransient<MainWindow>();
			services.AddTransient<CheckInForm>();
			services.AddTransient<ISqlDataAccess, SqlDataAccess>();
			services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			IConfiguration config = builder.Build();

			services.AddSingleton(config);

			string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
			if (dbChoice == "sql")
			{
				services.AddTransient<IDataBaseData, SqlData>();
			}
			else if (dbChoice == "sqlite")
			{
				services.AddTransient<IDataBaseData, SqliteData>();
			}
			else
			{
				services.AddTransient<IDataBaseData, SqlData>();
			}

			serviceProvider = services.BuildServiceProvider();
			var mainWindow = serviceProvider.GetService<MainWindow>();
			
			mainWindow.Show();
		}
	}
}
