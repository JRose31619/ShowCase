using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Data;
using SQLHMLibrary.Models;
using System;
using System.IO;

namespace SQLHomeWork
{
    public class Program
	{
		static void Main(string[] args)
		{
			SqlData sql = new SqlData(GetConnectionString());

            //ReadContact(sql, 1002);

            //CreateNewEntry(sql);

            //UpdatePerson(sql);

            //RemoveAddressFromPerson(sql, 1002,1002);


            Console.WriteLine("Done processing");
            Console.ReadLine();

		}

		private static void RemoveAddressFromPerson(SqlData sql, int personId, int addressId) 
		{
			sql.RemoveAddressFromPerson(personId, addressId);
		}

		private static void UpdatePerson(SqlData sql) 
		{
			BasicInfoModel person = new BasicInfoModel 
			{
				Id = 3,
				FirstName = "Olive",
				LastName = "Rose"

			};
			sql.UpdatePersonName(person);
		}

		private static void CreateNewEntry(SqlData sql) 
		{
			FullInfoModel user = new FullInfoModel
			{
				BasicInfo = new BasicInfoModel
				{
					FirstName = "Caleb",
					LastName = "Boyd"
				}

			};

			user.Addresses.Add(new AddressModel
			{
				StreetAddress = "804 E Pearl",
				City = "Miamisburg",
				State = "Ohio",
				ZipCode = "45342"
			});

			user.EmployerInfo = new EmployerModel
			{
				EmployerTitle = "General Manager"
			};

			sql.CreateEntry(user);
		}

		private static void ReadContact(SqlData sql, int personId) 
		{
			var contact = sql.GetContactById(personId);

            Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");
        }

		private static string GetConnectionString(string connectionStringName = "Default") 
		{
			string output = "";

			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

			var config = builder.Build();

			output = config.GetConnectionString(connectionStringName);

			return output;
		}
	}
}
