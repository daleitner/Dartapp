using Base;
using FileIO.XMLReader;
using SQLDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseInitializer
{
	public class ORDictionary
	{
		#region members
		#endregion

		#region ctors
		public ORDictionary(string orMappingFilePath)
		{
			OREntries = LoadORMappingFile(orMappingFilePath);
		}
		#endregion

		#region properties
		public List<OREntry> OREntries { get; set; }
		#endregion

		#region private methods
		private List<OREntry> LoadORMappingFile(string orMappingFilePath)
		{
			List<OREntry> ret = new List<OREntry>();
			if (System.IO.File.Exists(orMappingFilePath))
			{
				List<Node> xml = XMLReader.ReadXMLFile(orMappingFilePath);
				if (xml != null)
				{
					Node mappingsNode = xml.FirstOrDefault();
					if (mappingsNode != null && mappingsNode.Childs != null)
					{
						foreach (Node n in mappingsNode.Childs)
						{
							if (n.Name == "Table")
							{
								string table = n.Attributes["name"];
								string csObject = n.Attributes["csobject"];
								List<ORColumn> columns = new List<ORColumn>();
								if (n.Childs != null)
								{
									foreach (Node c in n.Childs)
									{
										if (c.Name == "Column")
										{
											string attribute = "";
											if(c.Attributes.Keys.Contains("attribute"))
												attribute = c.Attributes["attribute"];
											string column = c.Attributes["name"];
											string type = c.Attributes["type"];
											columns.Add(new ORColumn(attribute, column, type));
										}
									}
								}
								ret.Add(new OREntry(csObject, table, columns));
							}
						}
					}
				}
			}
			return ret;
		}
		#endregion

		#region public methods
		public OREntry GetEntryByTable(string tableName)
		{
			return this.OREntries.Where(x => x.RelationName == tableName).FirstOrDefault();
		}

		public OREntry GetEntryByObject(string csObjectName)
		{
			return this.OREntries.Where(x => x.ObjectName == csObjectName).FirstOrDefault();
		}

		public DataBaseTable GetTableByObject(Type t)
		{
			var entry = this.OREntries.Where(x => x.ObjectName == t.Name).FirstOrDefault();
			return new DataBaseTable(entry.RelationName, entry.Columns.Select(x => new DataBaseColumn(x.ColumnName, x.ColumnType)).ToList());
		}

		public DataBaseTable GetTableByRelation(string relationName)
		{ 
			var entry = this.OREntries.Where(x => x.RelationName == relationName).FirstOrDefault();
			return new DataBaseTable(entry.RelationName, entry.Columns.Select(x => new DataBaseColumn(x.ColumnName, x.ColumnType)).ToList());
		}

		public Dictionary<DataBaseColumn, object> CreateDatabaseDictionary(DataBaseTable table, ModelBase model)
		{
			var ret = new Dictionary<DataBaseColumn, object>();
			var entry = this.OREntries.Where(x => x.RelationName == table.Name).FirstOrDefault();
			foreach (var col in entry.Columns)
			{
				var column = table.Columns[col.ColumnName];
				object objectValue = null;
				if (col.AttributeName == "Id")
					objectValue = model.GetId();
				else
					objectValue = model.GetType().GetProperty(col.AttributeName).GetValue(model);
				ret.Add(column, objectValue);
			}
			return ret;
		}
		#endregion
 }
}