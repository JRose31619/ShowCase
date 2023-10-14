using Microsoft.Extensions.Configuration;
using EDBLIbrary.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using EDBLIbrary.Models;

namespace EDBWPF
{
	public class InputValidation
    {
		// left here on 10/14
		// bug testing is done for sql server
		// now to test sqlite and mysql
		// hopefully i'll be done with this soon
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
			else if (userInput.EmployerInfo.AccessCode != "" &&	
					 int.TryParse(userInput.EmployerInfo.AccessCode,out int validCode) == false)
			{
				MessageBox.Show("Only enter numbers for new access codes");
				output = false;
				return output;
			}
			else if (userInput.EmployerInfo.AccessCode.Count() > 10)
			{
				MessageBox.Show("Access codes cannot have more than ten numbers");
				output = false;
				return output;
			}
			else if (userInput.EmployerInfo.EmployerTitle.Count() > 50)
			{
				MessageBox.Show("Employer titles cannot have more than 50 characters");
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

		// potential place for refactoring
		// changes rolled back here
		public static bool ValidateExistingEmployee(string id,string db) 
		{
			bool output = false;
			if (int.TryParse(id, out int validId) == false)
			{
				output = false;
				MessageBox.Show("Id is not a valid number, please try again");
				return output;
			}

			UIActions action = new UIActions();

			FullInfoModel employeeData = new FullInfoModel();

			employeeData = action.RetrieveEmployeeInfo(id, db);

			try
			{
				if (employeeData == null)
				{
					throw new NullReferenceException();
				}
				else if (employeeData.BasicInfo.Id == validId)
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
			catch (Exception)
			{
				MessageBox.Show("No entry exists with provided Id");

			}

			return output;
		}
		// potential place for refactor
		public static bool ValidateExistingEmployerInfo(string id,string db) 
		{
			bool output = false;

			if (int.TryParse(id, out int validId) == false)
			{
				output = false;
				MessageBox.Show("Id is not a valid number, please try again");
				return output;
			}

			UIActions action = new UIActions();

			FullInfoModel employeeData = action.RetrieveEmployeeInfo(id, db);

			try
			{
				if (employeeData.EmployerInfo == null)
				{
					output = false;
					return output;
				}
				else
				{
					output = true;

					MessageBox.Show("This user already has employer information");

					return output;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

				return output;
			}
		}

		public static bool ValidateExistingAddress(string id, string db, string selectedAddress) 
		{
			AddressModel emptyModel = new AddressModel();

			bool output = ValidateExistingAddress(id, db,selectedAddress,emptyModel,true);

			return output;
		}
		public static bool ValidateExistingAddress(string id, string db, AddressModel address) 
		{
			string selectedAddress = "";

			bool output = ValidateExistingAddress(id,db,selectedAddress,address,false);

			return output;
		}

		public static bool ValidateExistingAddress(string id,
											 string db,
											 string selectedAddress,
											 AddressModel previousAddress,
											 bool hasSelectedAddress) 
		{
			bool output = false;

			UIActions action = new UIActions();

			FullInfoModel employeeData = action.RetrieveEmployeeInfo(id, db);

			int validNumber;

			bool successfulParse = int.TryParse(selectedAddress, out validNumber);

			if (successfulParse == false && selectedAddress != "" &&
				hasSelectedAddress == true)
			{
				MessageBox.Show("Selected address is not a valid number");
				
				return output;
			}
			else if (successfulParse == false && selectedAddress == "" &&
					 hasSelectedAddress == true)
			{
				MessageBox.Show("Address selection is empty. Please enter a number");

				return output;
			}

			try
			{
				if (previousAddress.Id == 0 && successfulParse == true
				&& employeeData.Addresses[validNumber - 1] != null && 
				hasSelectedAddress == true)
				{
					output = true;

					return output;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

				return output;
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
