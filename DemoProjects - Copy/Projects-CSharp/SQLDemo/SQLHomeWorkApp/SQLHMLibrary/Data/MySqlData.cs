using SQLHMLibrary.DataAccess;
using SQLHMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLHMLibrary.Data
{
    public class MySqlData : ISqlData
    {
        private readonly string _connectionString;
        MySqlDataAccess db = new MySqlDataAccess();

        public MySqlData(string connectionString)
        {
            _connectionString = connectionString;
        }

		public void AddNewAddress(EmployeeAddressModel newAddress)
		{
			string sql = @"insert into addresses (StreetAddress, City, State, ZipCode) 
                           values (@StreetAddress, @City, @State, @ZipCode);";
			db.SaveData(sql,
						new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
						_connectionString);

			sql = @"select Id from addresses where StreetAddress = @StreetAddress 
                    and City = @City and State = @State and ZipCode = @ZipCode";

			newAddress.AddressId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newAddress.StreetAddress, newAddress.City, newAddress.State, newAddress.ZipCode },
			_connectionString).First().Id;

			sql = "select Id from employees where FirstName = @FirstName and LastName = @LastName;";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newAddress.FirstName, newAddress.LastName },
			_connectionString).First().Id;

			sql = "insert into peopleaddress (PersonId, AddressId) values (@PersonId, @AddressId);";
			db.SaveData(sql, new { PersonId = personId, newAddress.AddressId }, _connectionString);
		}
		public void AddNewEmployerInfo(FullInfoModel newEmployerInfo)
		{
			string sql = @"insert into employers (EmployerTitle, AccessCode) 
                           values (@EmployerTitle, @AccessCode);";
			db.SaveData(sql,
						new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
						_connectionString);

			sql = @"select Id from employers where EmployerTitle = @EmployerTitle
                   and AccessCode = @AccessCode";

			newEmployerInfo.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.EmployerInfo.EmployerTitle, newEmployerInfo.EmployerInfo.AccessCode },
			_connectionString).First().Id;

			sql = "select Id from employees where FirstName = @FirstName and LastName = @LastName;";

			int personId = db.LoadData<IdLookupModel, dynamic>(sql,
			new { newEmployerInfo.BasicInfo.FirstName, newEmployerInfo.BasicInfo.LastName },
			_connectionString).First().Id;

			sql = "insert into peopleemployerLink (PersonId, EmployerTId) values (@PersonId, @Id);";
			db.SaveData(sql, new { PersonId = personId, newEmployerInfo.EmployerInfo.Id }, _connectionString);
		}
        public void CreateEntry(FullInfoModel person)
        {
            // Save the basic person
            string sql = "insert into employees (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql,
                        new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
                        _connectionString);

            // Get the Id of the person

            sql = "select Id from employees where FirstName = @FirstName and LastName = @LastName;";
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
                    sql = "insert into addresses (StreetAddress, City, State, ZipCode) values (@StreetAddress, @City, @State, @ZipCode);";
                    db.SaveData(sql,
                        new { address.StreetAddress, address.City, address.State, address.ZipCode },
                        _connectionString);

                    sql = "select Id from addresses where StreetAddress = @StreetAddress and City = @City and State = @State and ZipCode = @ZipCode";
                    address.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                        new { address.StreetAddress, address.City, address.State, address.ZipCode },
                        _connectionString).First().Id;
                }

                sql = "insert into peopleaddress (PersonId, AddressId) values (@PersonId, @AddressId);";
                db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
            }

            if (person.EmployerInfo.Id == 0)
            {
                sql = "insert into employers (EmployerTitle, AccessCode) values (@EmployerTitle, @AccessCode);";

                db.SaveData(sql, new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString);

                sql = "select Id from employers where EmployerTitle = @EmployerTitle and AccessCode = @AccessCode";
                person.EmployerInfo.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                new { person.EmployerInfo.EmployerTitle, person.EmployerInfo.AccessCode }, _connectionString).First().Id;
            }

            sql = "insert into peopleemployerlink (PersonId, EmployerTId) values (@PersonId, @EmployerTId);";
            db.SaveData(sql, new { PersonId = personId, EmployerTId = person.EmployerInfo.Id }, _connectionString);

            // Identify if the Address exists
            // Insert into the link table for the address
            // Insert the new address, if not, get the Id
            // Then do the link table insert
            // Do the same for the Employer info
        }
        public FullInfoModel GetContactById(int id)
        {
            string sql = "select Id, Firstname, LastName from employees where Id = @Id";

            FullInfoModel output = new FullInfoModel();

            output.BasicInfo = db.LoadData<BasicInfoModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                return null;
            }

            sql = @"select a.* 
					from addresses a
					inner join peopleaddress pa on pa.addressId = a.Id
					where pa.personId = @Id";

            output.Addresses = db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            sql = @"select e.* 
					from employers e
					inner join peopleemployerlink pe on pe.employerTId = e.Id
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

            string sql = "select Id, PersonId, AddressId from peopleaddress where AddressId = @AddressId;";
            var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
                new { AddressId = addressId },
                _connectionString);

            sql = "delete from peopleaddress where AddressId  = @AddressId and PersonId  = PersonId";
            db.SaveData(sql, new { PersonId = personId, AddressId = addressId }, _connectionString);

            if (links.Count == 1)
            {
                sql = "delete from addresses where Id = @AddressId;";
                db.SaveData(sql, new { AddressId = addressId }, _connectionString);
            }
        }
		public void RemoveAllEmployeeInfo(int companyId, int employerId)
		{
			if (employerId != 0)
			{
				RemoveEmployerInfo(companyId, employerId);
			}
			string sql = "select AddressId from peopleaddress where PersonId = @PersonId";

			List<AddressIdModel> addressIds = db.LoadData<AddressIdModel, dynamic>(sql,
																				 new { PersonId = companyId },
																				 _connectionString);

			foreach (AddressIdModel addressId in addressIds)
			{
				RemoveAddressFromPerson(companyId, addressId.AddressId);
			}

			sql = "delete from employees where Id = @Id";
			db.SaveData(sql, new { Id = companyId }, _connectionString);
		}
		public void RemoveEmployerInfo(int personId, int employerId)
		{
			string sql = @"select Id, PersonId, EmployerTId from peopleemployerLink 
                           where EmployerTId = @EmployerTId";

			var links = db.LoadData<TableLinkIdModel, dynamic>(sql,
															   new { EmployerTId = employerId },
															   _connectionString);

			sql = @"delete from peopleemployerLink 
                  where EmployerTId = @EmployerTId and PersonId = @PersonId";
			db.SaveData(sql, new { PersonId = personId, EmployerTId = employerId }, _connectionString);

			if (links.Count == 1)
			{
				sql = "delete from employers where Id = @EmployerTId";
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
			string sql = @"update employers set EmployerTitle = @EmployerTitle,
                           AccessCode = @AccessCode
                           where Id = @Id";
			db.SaveData(sql, newEmployerInfo, _connectionString);
		}
		public void UpdatePersonAddress(AddressModel newAddress)
		{
			string sql = @"update addresses set StreetAddress = @StreetAddress,
                          City = @City, State = @State, ZipCode = @ZipCode
                          where Id = @Id";
			db.SaveData(sql, newAddress, _connectionString);
		}
        public void UpdatePersonName(BasicInfoModel person)
        {
            string sql = "update employees set FirstName = @FirstName, LastName = @LastName where Id = @Id";
            db.SaveData(sql, person, _connectionString);
        }
	}
}
