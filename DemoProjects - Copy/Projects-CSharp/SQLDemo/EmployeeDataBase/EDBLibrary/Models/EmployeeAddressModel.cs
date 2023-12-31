﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EDBLIbrary.Models
{
	public class EmployeeAddressModel
	{
		public int AddressId { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
