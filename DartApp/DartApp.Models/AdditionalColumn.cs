using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;

namespace DartApp.Models
{
	public class AdditionalColumn : ModelBase
	{
		#region ctors
		public AdditionalColumn()
		{
			this.Name = "";
			this.Values = new List<AdditionalColumnValue>();
		}

		public AdditionalColumn(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Name = itemArray[1];
			this.Values = new List<AdditionalColumnValue>();
		}
		#endregion

		#region properties
		public string Name { get; set; }
		public List<AdditionalColumnValue> Values { get; set; }
		#endregion
 }
}