using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;

namespace DataBaseWPFUI
{
    public class InputValidation
    {
         public static bool ValidateNewUserEntry(FullInfoModel userInput) 
        {
			bool output;

			if ((userInput.EmployerInfo.EmployerTitle == "" && userInput.EmployerInfo.AccessCode != "")
				|| (userInput.EmployerInfo.EmployerTitle != "" && userInput.EmployerInfo.AccessCode == ""))
			{
				MessageBox.Show("Incomplete information entered for Manager fields");
				output = false;
				return output;
			}
			else if (int.TryParse(userInput.EmployerInfo.AccessCode,out int validCode) == false
					 || userInput.EmployerInfo.AccessCode.Count() > 10)
			{
				MessageBox.Show("Only enter numbers and ensure entered code is less then 10 characters");
				output = false;
				return output;
			}
			else if (ValidateSubmittedInfo(userInput) == false)
			{
				output = false;
				return output;
			}
			else 
			{
				output = true;
				return output;
			}
		}
		// potential method to get rid off/ refactor
		public static string ValidateDataBaseChoice(string db) 
		{

			if (db == "Current DateBase Is SqlServer")
			{
				db = "SqlServer";
				return db;
			}
			else if (db == "Current DateBase Is Sqlite")
			{
				db = "Sqlite";
				return db;
			}
			else if (db == "Current DateBase Is MySql")
			{
				db = "MySql";
				return db;
			}
			else
			{
				db = "Error";
				return db;
			}
		}
		// potential method to get rid off/ refactor
		public static bool ValidateNewAddress(EmployeeAddressModel newAddress) 
		{
			bool output;

			if (ValidateSubmittedInfo(newAddress) == false)
			{
				output = false;
				return output;
			}
			else 
			{
				output = true;
				return output; 
			}
		}
		public static bool ValidateSubmittedInfo(string submittedString) 
		{
			List<string> strings = new List<string>() 
			{
				submittedString
			};

			bool output = ValidateSubmittedInfo(strings);

			return output;
		}

		public static bool ValidateSubmittedInfo(EmployerModel newEmployerInfo) 
		{
			bool output;
			List<string> submittedInfo = new List<string>() 
			{
				newEmployerInfo.EmployerTitle,
				newEmployerInfo.AccessCode
			};

			output = ValidateSubmittedInfo(submittedInfo);

			return output;
		}
		public static bool ValidateSubmittedInfo(AddressModel address) 
		{
			List<string> submittedInfo = new List<string>() 
			{
				address.StreetAddress,
				address.City,
				address.State,
				address.ZipCode
			};

			bool output = ValidateSubmittedInfo(submittedInfo);

			return output;
		}

		public static bool ValidateSubmittedInfo(BasicInfoModel newName) 
		{
			List<string> submittedInfo = new List<string>() 
			{
				newName.FirstName,
				newName.LastName
			};

			bool output = ValidateSubmittedInfo(submittedInfo);

			return output;
		}

		public static bool ValidateSubmittedInfo(EmployeeAddressModel newAddress) 
		{
			List<string> userInfo = new List<string>() 
			{
				newAddress.FirstName,
				newAddress.LastName,
				newAddress.StreetAddress,
				newAddress.City,
				newAddress.State,
				newAddress.ZipCode,
			};

			bool output = ValidateSubmittedInfo(userInfo);

			return output;
		}

		public static bool ValidateSubmittedInfo(FullInfoModel newUser) 
		{
			// take everything in model and put it into list
			// pass to original method
			List<string> userInfo = new List<string>() 
			{
				newUser.BasicInfo.FirstName,
				newUser.BasicInfo.LastName,
				newUser.Addresses[0].StreetAddress,
				newUser.Addresses[0].City,
				newUser.Addresses[0].State,
				newUser.Addresses[0].ZipCode,
				newUser.EmployerInfo.EmployerTitle
			};
			bool output = ValidateSubmittedInfo(userInfo);

			return output;
		}

		public static bool ValidateSubmittedInfo(List<string> submittedInfo) 
		{
			bool output;

			foreach (string data in submittedInfo) 
			{
				if (data == "")
				{
					MessageBox.Show("Please fill out all required fields");
					output = false;
					return output;
				}
				else if (data.Count() > 50) 
				{
					MessageBox.Show("Please ensure all entered data is less than 50 characters!");
					output = false;
					return output;
				}
			}

			output = true;
			return output;
		}

		public static bool ValidateExistingEmployee(string id, string db) 
		{
			string firstName = "";

			string lastName = "";

			bool output = ValidateExistingEmployee(firstName,lastName,id,db);

			return output;
		}
		// potential place for refactoring
		public static bool ValidateExistingEmployee(string firstName,
											  string lastName,
											  string id,
											  string db) 
		{
			bool output;
			if (int.TryParse(id, out int validId) == false)
			{
				output = false;
				MessageBox.Show("Id is not a valid number, please try again");
				return output;
			}

			UIActions action = new UIActions();

			FullInfoModel employeeData = new FullInfoModel();

			employeeData = action.RetrieveEmployeeInfo(id, db);

			if (firstName == "" && lastName == "" && employeeData.BasicInfo.Id
				== validId)
			{
				output = true;
				return output;
			}
			else if (employeeData.BasicInfo.FirstName == firstName &&
				employeeData.BasicInfo.LastName == lastName)
			{
				output = true;
				return output;
			}
			else
			{
				output = false;
				return output;
			}
		}
		// potential place for refactor
		public static bool ValidateExistingEmployerInfo(string id,string db) 
		{
			bool output;

			if (int.TryParse(id, out int validId) == false)
			{
				output = false;
				MessageBox.Show("Id is not a valid number, please try again");
				return output;
			}

			UIActions action = new UIActions();

			FullInfoModel employeeData = action.RetrieveEmployeeInfo(id, db);

			if (employeeData.EmployerInfo == null)
			{
				output = false;
				return output;
			}
			else
			{
				output = true;
				return output;
			}
		}

		public static bool ValidateExistingAddress(string id, string db, string selectedAddress) 
		{
			AddressModel emptyModel = new AddressModel();

			bool output = ValidateExistingAddress(id, db,selectedAddress,emptyModel);

			return output;
		}
		public static bool ValidateExistingAddress(string id, string db, AddressModel address) 
		{
			string selectedAddress = "";

			bool output = ValidateExistingAddress(id,db,selectedAddress,address);

			return output;
		}
		// potential place for refactor
		public static bool ValidateExistingAddress(string id,string db,string selectedAddress,AddressModel previousAddress) 
		{
			bool output = false;

			UIActions action = new UIActions();

			FullInfoModel employeeData = action.RetrieveEmployeeInfo(id, db);

			if (previousAddress.Id == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					int address = int.Parse(selectedAddress);
					if (address == i && employeeData.Addresses[i - 1] != null)
					{
						output = true;

						return output;
					}
				}
			}

			foreach (var address in employeeData.Addresses)
			{
				if (previousAddress.StreetAddress == address.StreetAddress &&
					previousAddress.City == address.City && previousAddress.State ==
					address.State && previousAddress.ZipCode == address.ZipCode)
				{
					output = true;
					return output;
				}
				else
				{
					output = false;
				}
			}

			if (output == false)
			{
				MessageBox.Show("Given address doesn't exist in our records");
			}

			return output;
		}
}
}
