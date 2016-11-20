using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Menu
{
	public class DataViewModel
	{
		public DataViewModel()
		{
			this.Columns = new Dictionary<string, object>();
		}
		public int Placement { get; set; }
		public Dictionary<string, object> Columns { get; set; }
		public int Sum { get; set; }
		public string Name { get; set; }

		public object[] ToObjectArray()
		{
			var ret = new object[this.Columns.Count+3];
			ret[0] = this.Placement;
			ret[1] = this.Name;
			for (int i = 0; i < this.Columns.Count; i++)
			{
				ret[i + 2] = this.Columns.Values.ToList()[i];
			}
			ret[ret.Length - 1] = this.Sum;
			return ret;
		}
	}
}
