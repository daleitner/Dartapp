using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DartApp.Models
{
	public class PlacementPoint : ModelBase
	{
		#region members
		#endregion

		#region ctors
		public PlacementPoint()
		{
			Position = 0;
			Points = 0;
		}

		public PlacementPoint(int position, int points)
		{
			Position = position;
			Points = points;
		}

		public PlacementPoint(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Position = Int32.Parse(itemArray[1]);
			this.Points = Int32.Parse(itemArray[2]);
		}
		#endregion

		#region properties
		public int Position { get; set; }
		public int Points { get; set; }
		#endregion
 }
}