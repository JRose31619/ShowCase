using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySqlUI
{
    public static class CrudUIActions
	{
		public static ISqlData CreateCrud(string connectionStringName) 
		{

			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

			var config = builder.Build();

			var connectionString = config.GetConnectionString(connectionStringName);

			ISqlData sql;

			if (connectionStringName == "SqlServer")
			{
				sql = new SqlData(connectionString);
				return sql;
			}
			else if (connectionStringName == "Sqlite") 
			{
				sql = new SqliteData(connectionString);
				return sql;
			}
			else if (connectionStringName == "Default")
			{
				sql = new MySqlData(connectionString);
				return sql;
			}
			else 
			{
				throw new Exception("Inputted connection string is invalid");
			}

		}
		public static void RemoveAddressFromPerson(ISqlData sql)
		{
			//int personId, int addressId

			Console.WriteLine("Please enter your company Id");

			int personId = int.Parse(Console.ReadLine());

			Console.WriteLine("Please enter your address Id");

			int addressId = int.Parse(Console.ReadLine());

			sql.RemoveAddressFromPerson(personId, addressId);
		}
		public static void UpdatePerson(ISqlData sql)
		{
			Console.WriteLine("Please enter your company Id");

			int Id = int.Parse(Console.ReadLine());

			Console.WriteLine("Please enter your first name");

			string firstName = Console.ReadLine();

			Console.WriteLine("Please enter your last name");

			string lastName = Console.ReadLine();

			BasicInfoModel person = new BasicInfoModel
			{
				Id = Id,
				FirstName = firstName,
				LastName = lastName

			};
			sql.UpdatePersonName(person);
		}
		public static void CreateNewEntry(ISqlData sql)
		{

			Console.WriteLine("Please enter your first name");

			string firstName = Console.ReadLine();

			Console.WriteLine("Please enter your last name");

			string lastName = Console.ReadLine();

			FullInfoModel user = new FullInfoModel
			{
				BasicInfo = new BasicInfoModel
				{
					FirstName = firstName,
					LastName = lastName
				}

			};
			Console.WriteLine();

			Console.WriteLine("Please enter your street address");

			string streetAddress = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your city");

			string city = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your state");

			string state = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your zipcode");

			string zipCode = Console.ReadLine();

			Console.WriteLine();

			Console.Clear();

			user.Addresses.Add(new AddressModel
			{
				StreetAddress = streetAddress,
				City = city,
				State = state,
				ZipCode = zipCode
			});

			Console.WriteLine("Please enter your Employer Title if applicable");

			string employerTitle = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your Employer code if applicaable");

			int accessCode = int.Parse(Console.ReadLine());

			Console.WriteLine();

			user.EmployerInfo = new EmployerModel
			{
				EmployerTitle = employerTitle,
				AccessCode = accessCode.ToString(),
			};

			Console.Clear();

			sql.CreateEntry(user);
		}
		public static void AddNewAddress(ISqlData sql)
		{
			Console.WriteLine();

			Console.WriteLine("Please enter your first name");

			string firstName = Console.ReadLine(); Console.WriteLine();

			Console.WriteLine("Please enter your last name");

			string lastName = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your street address");

			string streetAddress = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your city");

			string city = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your state");

			string state = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your zipcode");

			string zipCode = Console.ReadLine();

			Console.WriteLine();

			Console.Clear();

			EmployeeAddressModel newAddress = new EmployeeAddressModel() 
			{
				StreetAddress = streetAddress,
				City = city,
				State = state,
				ZipCode = zipCode,
				FirstName = firstName,
				LastName = lastName
			};
			sql.AddNewAddress(newAddress);
		}
		public static void AddNewEmployerStatus(ISqlData sql) 
		{
			Console.WriteLine();

			Console.WriteLine("Please enter your first name");

			string firstName = Console.ReadLine(); Console.WriteLine();

			Console.WriteLine("Please enter your last name");

			string lastName = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your new employer title");

			string employerTitle = Console.ReadLine();

			Console.WriteLine();

			int accessCode;
			bool validInput;
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Access Code");

				validInput = int.TryParse(Console.ReadLine(), out accessCode);
			} while (validInput == false);

			FullInfoModel newEmployerInfo = new FullInfoModel()
			{
				BasicInfo = new BasicInfoModel
				{
					FirstName = firstName,
					LastName = lastName,
				},

				EmployerInfo = new EmployerModel()
				{
					EmployerTitle = employerTitle,
					AccessCode = accessCode.ToString()
				}
			};

			sql.AddNewEmployerInfo(newEmployerInfo);

		}
		public static void UpdateEmployeeAddress(ISqlData sql) 
		{
			int id;
			bool validInput;
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Company Id");

				validInput = int.TryParse(Console.ReadLine(), out id); 
			} while (validInput == false);

			Console.WriteLine();

			Console.WriteLine("Please enter your street address");

			string streetAddress = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your city");

			string city = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your state");

			string state = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your zipcode");

			string zipCode = Console.ReadLine();

			Console.WriteLine();

			Console.Clear();

			AddressModel newAddress = new AddressModel()
			{
				Id = id,
				StreetAddress = streetAddress,
				City = city,
				State = state,
				ZipCode = zipCode
			};

			sql.UpdatePersonAddress(newAddress);
		}
		public static void UpdateEmployerStatus(ISqlData sql) 
		{
			int id;
			bool validInput;
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Employer Id");

				validInput = int.TryParse(Console.ReadLine(), out id);
			} while (validInput == false);

			Console.WriteLine();

			Console.WriteLine("Please enter your new postion");

			string employerTitle = Console.ReadLine();

			Console.WriteLine();

			Console.WriteLine("Please enter your employer code");

			int employerCode;
			
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Employer code");

				validInput = int.TryParse(Console.ReadLine(), out employerCode);
			} while (validInput == false);

			EmployerModel newEmployerInfo = new EmployerModel()
			{
				Id = id,
				EmployerTitle = employerTitle,
				AccessCode = employerCode.ToString(),
			};
			sql.UpdateEmployerStatus(newEmployerInfo);
		}
		public static void ReadContact(ISqlData sql)
		{
            Console.WriteLine("What is your Company Id?");
			int personId;
			bool correctId = int.TryParse(Console.ReadLine(), out personId);
			while (correctId == false) 
			{
                Console.WriteLine("That is not a correct Id. Please try again");
				int.TryParse(Console.ReadLine(), out personId);

			}

            var contact = sql.GetContactById(personId);

			Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");
		}
		public static void DeleteEmployerInfo(ISqlData sql) 
		{
			int companyId;
			int employerId;
			bool validCompanyId;
			bool validEmployerId;
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Company Id");

				validCompanyId = int.TryParse(Console.ReadLine(), out companyId);

				Console.WriteLine();

				Console.WriteLine("Please enter your Employer Id");

				validEmployerId = int.TryParse(Console.ReadLine(), out employerId);
			} while (validCompanyId == false || validEmployerId == false);

			sql.RemoveEmployerInfo(companyId, employerId);
		}
		public static void DeleteAllEmployeeInfo(ISqlData sql) 
		{
			int companyId;
			int employerId;
			bool validCompanyId;
			bool validEmployerId;
			do
			{
				Console.WriteLine();

				Console.WriteLine("Please enter your Company Id");

				validCompanyId = int.TryParse(Console.ReadLine(), out companyId);

				Console.WriteLine();

				Console.WriteLine("Please enter your Employer Id");

				validEmployerId = int.TryParse(Console.ReadLine(), out employerId);
			} while (validCompanyId == false || validEmployerId == false);

			sql.RemoveAllEmployeeInfo(companyId, employerId);
		}
	}
}
