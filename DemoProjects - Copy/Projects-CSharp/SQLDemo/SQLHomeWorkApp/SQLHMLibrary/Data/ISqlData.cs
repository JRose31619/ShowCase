using SQLHMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLHMLibrary.Data
{
    public interface ISqlData
    {
        public void CreateEntry(FullInfoModel person);
        public void UpdatePersonName(BasicInfoModel person);
        public void RemoveAddressFromPerson(int personId, int addressId);
        public FullInfoModel GetContactById(int id);
		void UpdatePersonAddress(AddressModel newAddress);
		void UpdateEmployerStatus(EmployerModel newEmployerInfo);
		void AddNewAddress(EmployeeAddressModel newAddress);
		void AddNewEmployerInfo(FullInfoModel newEmployerInfo);
		void RemoveEmployerInfo(int personId, int employerId);
		void RemoveAllEmployeeInfo(int companyId, int employerId);
	}
}
