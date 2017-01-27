using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Menu
{
	public class DataViewModel
	{
		public DataViewModel()
		{
			this.Points = new Dictionary<string, object>();
			this.AdditionalColumns = new Dictionary<string, object>();
			this.LegRatios = new Dictionary<string, object>();
		}
		public int Placement { get; set; }
		public Dictionary<string, object> LegRatios { get; set; } 
		public Dictionary<string, object> Points { get; set; }
		public Dictionary<string, object> AdditionalColumns { get; set; }
		public int Sum { get; set; }
		public string Name { get; set; }

		public object[] ToObjectArray()
		{
			var ret = new object[this.Points.Count+this.LegRatios.Count+this.AdditionalColumns.Count+3];
			ret[0] = this.Placement;
			ret[1] = this.Name;
			for (int i = 0; i < this.AdditionalColumns.Count; i++)
			{
				ret[i + 2] = this.AdditionalColumns.Values.ToList()[i];
			}
			for (int i = 0; i < this.LegRatios.Count; i++)
			{
				ret[i + this.AdditionalColumns.Count + 2] = this.LegRatios.Values.ToList()[i];
			}
			for (int i = 0; i < this.Points.Count; i++)
			{
				ret[i + this.LegRatios.Count + this.AdditionalColumns.Count + 2] = this.Points.Values.ToList()[i];
			}
			ret[ret.Length - 1] = this.Sum;
			return ret;
		}
	}
}
