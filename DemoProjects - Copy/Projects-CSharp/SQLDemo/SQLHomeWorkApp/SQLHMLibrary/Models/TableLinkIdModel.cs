using System;
using System.Collections.Generic;
using System.Text;

namespace SQLHMLibrary.Models
{
	public class TableLinkIdModel
	{
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int EmployerId { get; set; }
    }
}
