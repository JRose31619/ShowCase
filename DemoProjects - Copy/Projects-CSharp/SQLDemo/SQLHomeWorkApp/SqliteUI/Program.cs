using Microsoft.Extensions.Configuration;
using SQLHMLibrary.Models;
using System;
using System.IO;
using SQLHMLibrary.Data;

namespace SqliteUI
{
    public class Program
	{
		static void Main(string[] args)
		{
			SqliteData sql = new SqliteData(GetConnectionString());

			//ReadContact(sql, 1);

			//CreateNewEntry(sql);

			//ReadContact(sql, 3);

			//UpdatePerson(sql);

            //gRemoveAddressFromPerson(sql, 3,3);

            Console.WriteLine("Done processing Sqlite");

            Console.ReadLine();
		}
		private static void RemoveAddressFromPerson(SqliteData sql, int personId, int addressId)
		{
			sql.RemoveAddressFromPerson(personId, addressId);
		}

		private static void UpdatePerson(SqliteData sql)
		{
			BasicInfoModel person = new BasicInfoModel
			{
				Id = 3,
				FirstName = "Doctor",
				LastName = "Boyd"

			};
			sql.UpdatePersonName(person);
		}

		private static void CreateNewEntry(SqliteData sql)
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
				EmployerTitle = "General Manager",
				EmployerId = 4253
			};

			sql.CreateEntry(user);
		}

		private static void ReadContact(SqliteData sql, int personId)
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
