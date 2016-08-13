using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Models
{
	public class Holiday : ModelBase
	{
		#region ctors
		public Holiday()
		{
		}

		public Holiday(Player player)
			: base()
		{
			this.Player = player;
		}

		public Holiday(List<string> itemArray)
		{
			this.Id = itemArray[0];
		}
		#endregion

		#region properties

		public Player Player { get; set; }
		#endregion
	}
}