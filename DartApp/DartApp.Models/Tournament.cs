using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace DartApp.Models
{
	public class Tournament : ModelBase
	{
		public Tournament()
		{
		}

		public Tournament(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Date = DateTime.Parse(itemArray[1]);
		}

		public DateTime Date { get; set; }
	}
}
