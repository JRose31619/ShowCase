using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlUI
{
    public static class ProcessUserInput
	{
		public static string GetUserDataBaseChoice()
		{
			string userInput = "";
			string connectionStringName = "";

			bool acceptableInput = false;
			do
			{

				userInput = Console.ReadLine();
				if (userInput.ToLower() == "sql server")
				{
					connectionStringName = "SqlServer";
					acceptableInput = true;
				}
				else if (userInput.ToLower() == "sqlite")
				{
					connectionStringName = "Sqlite";
					acceptableInput = true;
				}
				else if (userInput.ToLower() == "my sql")
				{
					connectionStringName = "Default";
					acceptableInput = true;
				}
				else
				{
					Console.WriteLine("That is not a valid answer, please try again");
					Console.WriteLine("(sql server, sqlite, my sql)");
				}
			} while (acceptableInput == false);

			return connectionStringName;
		}

		public static void GetUserActionChoice(string userInput, ISqlData sql) 
		{
			bool acceptableInput = false;
			do
			{
				if (userInput.ToLower() == "create" || userInput.ToLower() == "create user")
				{
					CrudUIActions.AddNewEmployerStatus(sql);
					acceptableInput = true;
					Console.Clear();
				}
				else if (userInput.ToLower() == "read" || userInput.ToLower() == "read user")
				{
					Console.Clear();
					CrudUIActions.ReadContact(sql);
					acceptableInput = true;
				}
				else if (userInput.ToLower() == "update" || userInput.ToLower() == "update user")
				{
					CrudUIActions.UpdateEmployeeAddress(sql);
					acceptableInput = true;
					Console.Clear();
				}
				else if (userInput.ToLower() == "delete" || userInput.ToLower() == "delete user information")
				{
					CrudUIActions.DeleteAllEmployeeInfo(sql);
					acceptableInput = true;
					Console.Clear();
				}
				else
				{
					Console.WriteLine("That is not a valid choice. Please try again");
					Console.WriteLine("Create User/ Delete User information/ Update User/ Read User");

					userInput = Console.ReadLine();
				} 
			} while (acceptableInput == false);
		}

		public static bool GetUserChoiceToContinue() 
		{
			bool output = true;

			Console.WriteLine("Would you like to continue working?");
			Console.WriteLine("yes/no");


			string userInput = Console.ReadLine();
			if (userInput.ToLower() == "no")
			{
				output = false;
				Console.Clear();
			}
			else if (userInput.ToLower() == "yes")
			{

				Console.Clear();
			}
			else
			{
				output = false;
				Console.Clear();
			}

			return output;
		}
	}
}
