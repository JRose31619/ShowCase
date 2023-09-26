using System;
using System.Collections.Generic;
using System.Text;

namespace EDBLIbrary.Models
{
	public class FullInfoModel
	{
		public BasicInfoModel BasicInfo { get; set; }

		public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();

		public EmployerModel EmployerInfo { get; set; }
	}
}
