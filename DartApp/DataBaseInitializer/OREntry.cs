using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseInitializer
{
	public class OREntry
	{
		#region members
		#endregion

		#region ctors
		public OREntry()
		{
			this.RelationName = "";
			this.Columns = null;
			this.ObjectName = null;
		}

		public OREntry(string objectName, string relationName, List<ORColumn> columns)
		{
			this.RelationName = relationName;
			this.Columns = columns;
			this.ObjectName = objectName;
		}
		#endregion

		#region properties
		public string RelationName { get; set; }
		public List<ORColumn> Columns { get; set; }
		public string ObjectName { get; set; }
		#endregion

		#region private methods
		#endregion

		#region public methods
		#endregion
 }
}