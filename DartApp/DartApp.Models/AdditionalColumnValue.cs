using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;

namespace DartApp.Models
{
	public class AdditionalColumnValue : ModelBase
	{
		#region ctors
		public AdditionalColumnValue()
		{
			this.Player = null;
			this.Value = 0;
		}

		public AdditionalColumnValue(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Value = Int32.Parse(itemArray[1]);
		}
		#endregion

		#region properties
		public Player Player { get; set; }
		public int Value { get; set; }
		#endregion
 }
}