﻿using EDBLIbrary.DataAccess;
using EDBLIbrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDBLIbrary.Data
{
	public class SqliteData : ISqlData
	{
		private readonly string _connectionString;
		SqliteDataAccess db = new SqliteDataAccess();

		public SqliteData(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void AddNewAddress(EmployeeAddressModel newAddress)
		{
			string sql = @"insert into Addresses (StreetAddress, City, State, ZipCode) 
                           values (@StreetAddress, @City, @State, @ZipCode);";
			db.SaveData(sql,
						new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
						_connectionString);

			sql = @"select Id from Addresses where StreetAddress = @StreetAddress 
                    and City = @City and State = @State and ZipCode = @ZipCode";

			newAddress.AddressId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
			_connectionString).First().Id;

			sql = "select Id from Employees where Id = @Id;";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { Id = newAddress.PersonId },
			_connectionString).First().Id;

			sql = "insert into PeopleAddresses (PersonId, AddressId) values (@PersonId, @AddressId);";
			db.SaveData(sql, new { PersonId = personId, newAddress.AddressId }, _connectionString);
		}
		public void AddNewEmployerInfo(FullInfoModel newEmployerInfo)
		{
			string sql = @"insert into Employers (EmployerTitle, AccessCode) 
                           values (@EmployerTitle, @AccessCode);";
			db.SaveData(sql,
						new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
						_connectionString);

			sql = @"select Id from Employers where EmployerTitle = @EmployerTitle
                   and AccessCode = @AccessCode";

			newEmployerInfo.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
			_connectionString).First().Id;

			sql = "select Id from Employees where Id = @Id;";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.BasicInfo.Id},
			_connectionString).First().Id;

			sql = "insert into PeopleEmployerLink (PersonId, EmployerTId) values (@PersonId, @Id);";
			db.SaveData(sql, new { PersonId = personId, newEmployerInfo.EmployerInfo.Id }, _connectionString);
		}
		public void CreateEntry(FullInfoModel person)
		{
			string sql = "insert into Employees (FirstName, LastName) values (@FirstName, @LastName);";
			db.SaveData(sql,
						new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
						_connectionString);


			sql = "select Id from Employees where FirstName = @FirstName and LastName = @LastName;";
			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
				new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
				_connectionString).First().Id;

			foreach (var address in person.Addresses)
			{
				if (address.Id == 0)
				{
					sql = "insert into Addresses (StreetAddress, City, State, ZipCode) values (@StreetAddress, @City, @State, @ZipCode);";
					db.SaveData(sql,
						new { address.StreetAddress, address.City, address.State, address.ZipCode },
						_connectionString);

					sql = "select Id from Addresses where StreetAddress = @StreetAddress and City = @City and State = @State and ZipCode = @ZipCode";
					address.Id = db.LoadData<IdLookupModel, dynamic>(sql,
						new { address.StreetAddress, address.City, address.State, address.ZipCode },
						_connectionString).First().Id;

					sql = "insert into PeopleAddresses (PersonId, AddressId) values (@PersonId, @AddressId);";
					db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
				}
			}

			if (person.EmployerInfo.Id == 0 && person.EmployerInfo.EmployerTitle != "" &&
				person.EmployerInfo.AccessCode != "")
			{
				sql = "insert into Employers (EmployerTitle, AccessCode) values (@EmployerTitle, @AccessCode);";

				db.SaveData(sql, new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString);

				sql = "select Id from Employers where EmployerTitle = @EmployerTitle and AccessCode = @AccessCode";
				person.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
				new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString).First().Id;

				sql = "insert into PeopleEmployerLink (PersonId, EmployerTId) values (@PersonId, @EmployerTId);";
				db.SaveData(sql, new { PersonId = personId, EmployerTId = person.EmployerInfo.Id }, _connectionString);
			}
		}
		public FullInfoModel GetContactById(int id)
		{
			string sql = "select Id, Firstname, LastName from Employees where Id = @Id";

			FullInfoModel output = new FullInfoModel();

			output.BasicInfo = db.LoadData<BasicInfoModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

			if (output.BasicInfo == null)
			{
				return null;
			}

			sql = @"select a.* 
					from Addresses a
					inner join PeopleAddresses pa on pa.AddressId = a.Id
					where pa.personId = @Id";

			output.Addresses = db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString);

			sql = @"select e.* 
					from Employers e
					inner join PeopleEmployerLink pe on pe.EmployerTId = e.Id
					where pe.personId = @Id";

			output.EmployerInfo = db.LoadData<EmployerModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

			if (output.EmployerInfo == null)
			{
				output.EmployerInfo = null;
			}

			return output;
		}
		public void RemoveAddressFromPerson(int personId, int addressId)
		{
			string sql = "select Id, PersonId, AddressId from PeopleAddresses where AddressId = @AddressId;";
			var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
				new { AddressId = addressId },
				_connectionString);

			sql = "delete from PeopleAddresses where AddressId  = @AddressId and PersonId  = PersonId";
			db.SaveData(sql, new { PersonId = personId, AddressId = addressId }, _connectionString);

			if (links.Count == 1)
			{
				sql = "delete from Addresses where Id = @AddressId;";
				db.SaveData(sql, new { AddressId = addressId }, _connectionString);
			}
		}
		public void RemoveAllEmployeeInfo(int companyId, int employerId)
		{
			if (employerId != 0)
			{
				RemoveEmployerInfo(companyId, employerId);
			}
			string sql = "select AddressId from PeopleAddresses where PersonId = @PersonId";

			List<AddressIdModel> addressIds = db.LoadData<AddressIdModel, dynamic>(sql,
																				 new { PersonId = companyId },
																				 _connectionString);

			foreach (AddressIdModel addressId in addressIds)
			{
				RemoveAddressFromPerson(companyId, addressId.AddressId);
			}

			sql = "delete from Employees where Id = @Id";
			db.SaveData(sql, new { Id = companyId }, _connectionString);
		}
		public void RemoveEmployerInfo(int personId, int employerId)
		{
			string sql = @"select Id, PersonId, EmployerTId from PeopleEmployerLink 
                           where EmployerTId = @EmployerTId";

			var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
															   new { EmployerTId = employerId },
															   _connectionString);

			sql = @"delete from PeopleEmployerLink 
                  where EmployerTId = @EmployerTId and PersonId = @PersonId";
			db.SaveData(sql, new { PersonId = personId, EmployerTId = employerId }, _connectionString);

			if (links.Count == 1)
			{
				sql = "delete from Employers where Id = @EmployerTId";
				db.SaveData(sql, new { EmployerTId = employerId }, _connectionString);
			}
			else if (links.Count > 1)
			{
				throw new Exception("Too many Employer records for user");
			}
			else
			{
				throw new Exception("No employer records found");
			}
		}
		public void UpdateEmployerStatus(EmployerModel newEmployerInfo)
		{
			string sql = @"update Employers set EmployerTitle = @EmployerTitle,
                           AccessCode = @AccessCode
                           where Id = @Id";
			db.SaveData(sql, newEmployerInfo, _connectionString);
		}
		public void UpdatePersonAddress(AddressModel newAddress)
		{
			string sql = @"update Addresses set StreetAddress = @StreetAddress,
                          City = @City, State = @State, ZipCode = @ZipCode
                          where Id = @Id";
			db.SaveData(sql, newAddress, _connectionString);
		}
		public void UpdatePersonName(BasicInfoModel person)
		{
			string sql = "update Employees set FirstName = @FirstName, LastName = @LastName where Id = @Id";
			db.SaveData(sql, person, _connectionString);
		}
	}
}
