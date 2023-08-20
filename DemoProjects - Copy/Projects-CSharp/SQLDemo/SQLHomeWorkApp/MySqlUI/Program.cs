using SQLHMLibrary.Models;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Data;

namespace MySqlUI
{
    public class Program
	{
		static void Main(string[] args)
		{
			bool userUsingDB = true;
			do
			{
				Console.WriteLine("Which database would you like access to?");
				Console.WriteLine("(sql server, sqlite, my sql)");

				ISqlData sql = CrudUIActions.CreateCrud(ProcessUserInput.GetUserDataBaseChoice());

				Console.WriteLine("Great! What action would you like to perform?");
				Console.WriteLine("Create User/ Delete User information/ Update User/ Read User");

				ProcessUserInput.GetUserActionChoice(Console.ReadLine(), sql);

				userUsingDB = ProcessUserInput.GetUserChoiceToContinue();


            } while (userUsingDB == true);

            //MySqlCrud sql = new MySqlCrud(GetConnectionString(GetUserDataBaseChoice()));

            //ReadContact(sql, 1002);

            //CreateNewEntry(sql);

            //ReadContact(sql, 2);

            //UpdatePerson(sql);

            //ReadContact(sql, 2);

            //RemoveAddressFromPerson(sql, 2,2);

            Console.WriteLine("Done processing");

			Console.ReadLine();
		}

		//private static void RemoveAddressFromPerson(ISqlCrud sql, int personId, int addressId)
		//{
		//	sql.RemoveAddressFromPerson(personId, addressId);
		//}

		//private static void UpdatePerson(ISqlCrud sql)
		//{
		//	BasicPeopleModel person = new BasicPeopleModel
		//	{
		//		Id = 2,
		//		FirstName = "Doctor",
		//		LastName = "Boyd"

		//	};
		//	sql.UpdatePersonName(person);
		//}

		//private static void CreateNewEntry(ISqlCrud sql)
		//{
		//	FullPeopleModel user = new FullPeopleModel
		//	{
		//		BasicInfo = new BasicPeopleModel
		//		{
		//			FirstName = "Caleb",
		//			LastName = "Boyd"
		//		}

		//	};

		//	user.Addresses.Add(new AddressModel
		//	{
		//		StreetAddress = "804 E Pearl",
		//		City = "Miamisburg",
		//		State = "Ohio",
		//		ZipCode = "45342"
		//	});

		//	user.EmployerInfo = new EmployerModel
		//	{
		//		EmployerTitle = "General Manager",
		//		EmployerId = 4253
		//	};

		//	sql.CreateEntry(user);
		//}

		//private static void ReadContact(ISqlCrud sql, int personId)
		//{
		//	var contact = sql.GetContactById(personId);

		//	Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");
		//}


		//private static (string, string) GetConnectionString(string connectionStringName)
		//{

		//	string connectionString = "";

		//	var builder = new ConfigurationBuilder()
		//	.SetBasePath(Directory.GetCurrentDirectory())
		//	.AddJsonFile("appsettings.json");

		//	var config = builder.Build();

		//	connectionString = config.GetConnectionString(connectionStringName);

		//	return (connectionString, connectionStringName);
		//}
	}
	
}
