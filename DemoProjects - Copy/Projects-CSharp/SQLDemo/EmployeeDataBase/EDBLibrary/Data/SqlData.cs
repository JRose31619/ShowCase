using EDBLIbrary.DataAccess;
using EDBLIbrary.Models;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace EDBLIbrary.Data
{
	public class SqlData : ISqlData
	{
		private readonly string _connectionString;
		SqlDataAccess db = new SqlDataAccess();

		public SqlData(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void AddNewAddress(EmployeeAddressModel newAddress)
		{
			string sql = @"insert into dbo.Address (StreetAddress, City, State, ZipCode) 
                           values (@StreetAddress, @City, @State, @ZipCode);";
			db.SaveData(sql,
						new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
						_connectionString);

			sql = @"select Id from dbo.Address where StreetAddress = @StreetAddress 
                    and City = @City and State = @State and ZipCode = @ZipCode";

			newAddress.AddressId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
			_connectionString).First().Id;

			// Make these changes to other sql data
			//sql = "select Id from dbo.Employees where FirstName = @FirstName and LastName = @LastName;";
			sql = "select Id from dbo.Employees where Id = @Id";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new {Id = newAddress.PersonId},
			_connectionString).First().Id;

			sql = "insert into dbo.PeopleAddress (PersonId, AddressId) values (@PersonId, @AddressId);";
			db.SaveData(sql, new { PersonId = personId, newAddress.AddressId }, _connectionString);
		}
		public void AddNewEmployerInfo(FullInfoModel newEmployerInfo)
		{
			string sql = @"insert into dbo.Employer (EmployerTitle, AccessCode) 
                           values (@EmployerTitle, @AccessCode);";
			db.SaveData(sql,
						new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
						_connectionString);

			sql = @"select Id from dbo.Employer where EmployerTitle = @EmployerTitle
                   and AccessCode = @AccessCode";

			newEmployerInfo.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
			_connectionString).First().Id;

			sql = "select Id from dbo.Employees where FirstName = @FirstName and LastName = @LastName;";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.BasicInfo.FirstName, newEmployerInfo.BasicInfo.LastName },
			_connectionString).First().Id;

			sql = "insert into dbo.PeopleEmployerLink (PersonId, EmployerTId) values (@PersonId, @Id);";
			db.SaveData(sql, new { PersonId = personId, newEmployerInfo.EmployerInfo.Id }, _connectionString);
		}
		public void CreateEntry(FullInfoModel person)
		{
			// Save the basic person
			string sql = "insert into dbo.Employees (FirstName, LastName) values (@FirstName, @LastName);";
			db.SaveData(sql,
						new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
						_connectionString);

			// Get the Id of the person

			sql = "select Id from dbo.Employees where FirstName = @FirstName and LastName = @LastName;";
			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
				new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
				_connectionString).First().Id;

			// go over the Id of every address in the Addresses list property
			// see if the id for each is == 0. meaning the id hasnt been set yet
			// if it hasnt been set yet then insert the address data into the table
			// then load the data again to set the Model id to the id made in the table
			foreach (var address in person.Addresses)
			{
				if (address.Id == 0)
				{
					sql = "insert into dbo.Address (StreetAddress, City, State, ZipCode) values (@StreetAddress, @City, @State, @ZipCode);";
					db.SaveData(sql,
						new { address.StreetAddress, address.City, address.State, address.ZipCode },
						_connectionString);

					sql = "select Id from dbo.Address where StreetAddress = @StreetAddress and City = @City and State = @State and ZipCode = @ZipCode";
					address.Id = db.LoadData<IdLookupModel, dynamic>(sql,
						new { address.StreetAddress, address.City, address.State, address.ZipCode },
						_connectionString).First().Id;

					sql = "insert into dbo.PeopleAddress (PersonId, AddressId) values (@PersonId, @AddressId);";
					db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
				}

				//sql = "insert into dbo.PeopleAddress (PersonId, AddressId) values (@PersonId, @AddressId);";
				//db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
			}

			if (person.EmployerInfo.Id == 0 && person.EmployerInfo.EmployerTitle != "" &&
				person.EmployerInfo.AccessCode != "")
			{
				sql = "insert into dbo.Employer (EmployerTitle, AccessCode) values (@EmployerTitle, @AccessCode);";

				db.SaveData(sql, new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString);

				sql = "select Id from dbo.Employer where EmployerTitle = @EmployerTitle and AccessCode = @AccessCode";
				person.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
				new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString).First().Id;

				sql = "insert into dbo.PeopleEmployerLink (PersonId, EmployerTId) values (@PersonId, @EmployerTId);";
				db.SaveData(sql, new { PersonId = personId, EmployerTId = person.EmployerInfo.Id }, _connectionString);
			}
			//changes rolled back here

			//sql = "insert into dbo.PeopleEmployerLink (PersonId, EmployerTId) values (@PersonId, @EmployerTId);";
			//db.SaveData(sql, new { PersonId = personId, EmployerTId = person.EmployerInfo.Id }, _connectionString);

			// Identify if the Address exists
			// Insert into the link table for the address
			// Insert the new address, if not, get the Id
			// Then do the link table insert
			// Do the same for the Employer info
		}
		public FullInfoModel GetContactById(int id)
		{
			string sql = "select Id, Firstname, LastName from dbo.Employees where Id = @Id";

			FullInfoModel output = new FullInfoModel();

			output.BasicInfo = db.LoadData<BasicInfoModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

			if (output.BasicInfo == null)
			{
				return null;
			}

			sql = @"select a.* 
					from dbo.Address a
					inner join dbo.PeopleAddress pa on pa.AddressId = a.Id
					where pa.personId = @Id";

			output.Addresses = db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString);

			sql = @"select e.* 
					from dbo.Employer e
					inner join dbo.PeopleEmployerLink pe on pe.EmployerTId = e.Id
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
			// Find all usages of the addressId
			// If 1, delete the link and address
			// If > 1, delete link fo the person

			string sql = "select Id, PersonId, AddressId from dbo.PeopleAddress where AddressId = @AddressId;";
			var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
				new { AddressId = addressId },
				_connectionString);

			sql = "delete from dbo.PeopleAddress where AddressId  = @AddressId and PersonId  = PersonId";
			db.SaveData(sql, new { PersonId = personId, AddressId = addressId }, _connectionString);

			if (links.Count == 1)
			{
				sql = "delete from dbo.Address where Id = @AddressId;";
				db.SaveData(sql, new { AddressId = addressId }, _connectionString);
			}
		}
		public void RemoveAllEmployeeInfo(int companyId, int employerId)
		{
			if (employerId != 0)
			{
				RemoveEmployerInfo(companyId, employerId);
			}
			string sql = "select AddressId from dbo.PeopleAddress where PersonId = @PersonId";

			List<AddressIdModel> addressIds = db.LoadData<AddressIdModel, dynamic>(sql,
																				 new { PersonId = companyId },
																				 _connectionString);

			foreach (AddressIdModel addressId in addressIds)
			{
				RemoveAddressFromPerson(companyId, addressId.AddressId);
			}

			sql = "delete from dbo.Employees where Id = @Id";
			db.SaveData(sql, new { Id = companyId }, _connectionString);
		}
		public void RemoveEmployerInfo(int personId, int employerId)
		{
			string sql = @"select Id, PersonId, EmployerTId from dbo.PeopleEmployerLink 
                           where EmployerTId = @EmployerTId";

			var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
															   new { EmployerTId = employerId },
															   _connectionString);

			sql = @"delete from dbo.PeopleEmployerLink 
                  where EmployerTId = @EmployerTId and PersonId = @PersonId";
			db.SaveData(sql, new { PersonId = personId, EmployerTId = employerId }, _connectionString);

			if (links.Count == 1)
			{
				sql = "delete from dbo.Employer where Id = @EmployerTId";
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
			// Delete record from employer table
			// Delete record from Link table
		}
		public void UpdateEmployerStatus(EmployerModel newEmployerInfo)
		{
			string sql = @"update dbo.Employer set EmployerTitle = @EmployerTitle,
                           AccessCode = @AccessCode
                           where Id = @Id";
			db.SaveData(sql, newEmployerInfo, _connectionString);
		}
		public void UpdatePersonAddress(AddressModel newAddress)
		{
			string sql = @"update dbo.Address set StreetAddress = @StreetAddress,
                          City = @City, State = @State, ZipCode = @ZipCode
                          where Id = @Id";
			db.SaveData(sql, newAddress, _connectionString);
		}
		public void UpdatePersonName(BasicInfoModel person)
		{
			string sql = "update dbo.Employees set FirstName = @FirstName, LastName = @LastName where Id = @Id";
			db.SaveData(sql, person, _connectionString);
		}
	}
}
