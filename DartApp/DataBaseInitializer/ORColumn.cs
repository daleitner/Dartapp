using SQLDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseInitializer
{
	public class ORColumn
	{
		#region members
		#endregion

		#region ctors
		public ORColumn()
		{
			AttributeName = "";
			ColumnName = "";
			ColumnType = ColumnType.VARCHAR;
		}

		public ORColumn(string attributeName, string columnName, string columnType)
		{
			this.AttributeName = attributeName;
			this.ColumnName = columnName;
			ColumnType help;
			Enum.TryParse<ColumnType>(columnType, out help);
			this.ColumnType = help;
		}
		#endregion

		#region properties
		public string AttributeName { get; set; }
		public string ColumnName { get; set; }
		public ColumnType ColumnType { get; set; }
		#endregion

		#region private methods
		#endregion

		#region public methods
		#endregion
 }
}