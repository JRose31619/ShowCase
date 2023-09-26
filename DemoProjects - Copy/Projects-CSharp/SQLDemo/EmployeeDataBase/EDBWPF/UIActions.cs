using EDBLIbrary.Data;
using EDBLIbrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;

namespace EDBWPF
{
	public class UIActions
	{
		public ISqlData CreateSqlData(string db) 
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

			var config = builder.Build();

			var connectionString = config.GetConnectionString(db);

			ISqlData sql;

			if (db == "SqlServer")
			{
				sql = new SqlData(connectionString);
			}
			else if (db == "Sqlite")
			{
				sql = new SqliteData(connectionString);
			}
			else if (db == "MySql") 
			{
				sql = new MySqlData(connectionString);
			}
			else 
			{
				throw new Exception("Invalid connection string");
			}

			return sql;
		}
		// refactor to have this method anywhere that needs to pull from sql
		// left here on 8/19. debating on how to implement try catch blocks
		public FullInfoModel RetrieveEmployeeInfo(string id,string db) 
		{
			FullInfoModel employeeInfo = new FullInfoModel();

			int idValid = int.Parse(id);

			ISqlData sql = CreateSqlData(db);

			try
			{
				employeeInfo = sql.GetContactById(idValid);
				if (employeeInfo == null)
				{
					throw new Exception("No entry exists with the Id provided");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				
			}

			return employeeInfo;
		}

		public void DeleteAddress(string db, string userId, AddressModel selectedAddress) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.RemoveAddressFromPerson(int.Parse(userId), selectedAddress.Id);

			MessageBox.Show("Address deleted successfully!");
		}

		public void DeleteAllInfo(string db, int companyId, int employerId) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.RemoveAllEmployeeInfo(companyId,employerId);

			MessageBox.Show("All employee information deleted successfully!");
		}

		public void DeleteEmployerInfo(int companyId,int employerId, string db) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.RemoveEmployerInfo(companyId,employerId);

			MessageBox.Show("Employer information was deleted successfully!");
		}

		public void InsertNewUser(string db,FullInfoModel newUser) 
		{

			ISqlData sql = CreateSqlData(db);

			sql.CreateEntry(newUser);
		}

		public void InsertNewAddress(string db, EmployeeAddressModel newAddress) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.AddNewAddress(newAddress);

			MessageBox.Show("New address added successfully!");
		}

		public void InsertNewEmployerInfo(string db, FullInfoModel newEmployerInfo) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.AddNewEmployerInfo(newEmployerInfo);

			MessageBox.Show("New employer information added successfully!");
		}

		public int RetreiveEmployerKey(string id, string db) 
		{
			int output = 0;

			ISqlData sql = CreateSqlData(db);

			FullInfoModel employeeData = sql.GetContactById(int.Parse(id));

			if (int.Parse(id) == employeeData.BasicInfo.Id)
			{
				output = employeeData.EmployerInfo.Id;
				return output;
			}
			else
			{
				// do something different here
				throw new Exception("Given id didn't matched retrieved id");
			}
		}

		public int RetrievePrimaryKey(string id, string db) 
		{
			int output = 0;

			ISqlData sql = CreateSqlData(db);

			FullInfoModel employeeData = sql.GetContactById(int.Parse(id));

			if (int.Parse(id) == employeeData.BasicInfo.Id) 
			{
				output = employeeData.BasicInfo.Id;
				return output;
			}
			else 
			{
				// do something different here
				throw new Exception("Given id didn't matched retrieved id");
			}
		}

		public int RetrieveAddressKey(string id, string db, AddressModel previousAddress) 
		{
			int output = 0;

			ISqlData sql = CreateSqlData(db);

			FullInfoModel employeeData = sql.GetContactById(int.Parse(id));

			foreach (var address in employeeData.Addresses)
			{
				if (previousAddress.StreetAddress == address.StreetAddress &&
					previousAddress.City == address.City && previousAddress.State ==
					address.State && previousAddress.ZipCode == address.ZipCode)
				{
					output = address.Id;
					return output;
				}
			}

			if (output == 0)
			{
				throw new Exception("No address matches user input");
			}

			return output;
		}

		public void UpdateAddress(string db, AddressModel newAddress) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.UpdatePersonAddress(newAddress);

			MessageBox.Show("Address updated successfully!");
		}

		public void UpdateEmployerInfo(string db, EmployerModel newEmployerInfo) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.UpdateEmployerStatus(newEmployerInfo);

			MessageBox.Show("Employer information updated successfully!");
		}

		public void UpdateName(string db, BasicInfoModel newName) 
		{
			ISqlData sql = CreateSqlData(db);

			sql.UpdatePersonName(newName);

			MessageBox.Show("Name has been updated successfully!");
		}
	}
}
