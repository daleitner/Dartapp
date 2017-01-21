using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileIO.FileReader;
using FileIO.XMLReader;
using SQLDatabase;
using Base;

namespace DataBaseInitializer
{
	public class DataBaseManager
	{
		#region members
		private static DataBaseManager dbi;
		private readonly string setup = "";
		private readonly string testValueFile = "";
		private readonly string mappingPath = "";
		private string databaseName = "";
		#endregion

		#region ctor
		private DataBaseManager(string configFile, string mappingFile, string initialValuesFile)
		{
			this.setup = configFile;
			this.testValueFile = initialValuesFile;
			this.mappingPath = mappingFile;
			Initialize();
		}

		private void Initialize()
		{
			var dbconfig = ReadSetup(this.setup);
			var testValueCommands = ReadTestValues(this.testValueFile);
			if (dbconfig != null)
				this.DataBaseConnection = new DataBaseConnection(dbconfig.Keys.First(), dbconfig.Values.First(), testValueCommands);
			this.Mapping = new ORDictionary(this.mappingPath);
		}
		#endregion

		#region properties
		public DataBaseConnection DataBaseConnection { get; set; }
		public ORDictionary Mapping { get; set; }
		#endregion

		#region static methods
		public static DataBaseManager GetInstance(string configFile, string mappingFile, string initialValuesFile)
		{
			return dbi ?? (dbi = new DataBaseManager(configFile, mappingFile, initialValuesFile));
		}

		public static DataBaseManager GetInstance()
		{
			return dbi;
		}

		#endregion

		#region private methods
		private Dictionary<string, List<DataBaseTable>> ReadSetup(string configFile)
		{
			if (File.Exists(configFile))
			{
				var ret = new Dictionary<string, List<DataBaseTable>>();
				var dbTables = new List<DataBaseTable>();
				var xml = XMLReader.ReadXMLFile(configFile);

				Node dataBaseNode = null;
                if(xml != null)
                   dataBaseNode = xml.FirstOrDefault();
				if (dataBaseNode == null)
					return null;
				this.databaseName = dataBaseNode.Attributes["name"];
				var dataBase = dataBaseNode.Attributes[dataBaseNode.Attributes.Keys.First()];
				List<Node> tables = null;
				if (dataBaseNode.Childs != null)
				{
					foreach (var n in dataBaseNode.Childs.Where(n => n.Name == "Tables"))
					{
						tables = n.Childs;
						break;
					}
				}

				if (tables == null)
					return null;

				dbTables.AddRange(tables.Select(NodeToDataBaseTable));
				ret.Add(dataBase, dbTables);
				return ret;
			}
			return null;
		}


		private List<string> ReadTestValues(string p)
		{
			if (File.Exists(p))
			{
				var f = FileReader.ReadFile(p);
				var lines = f.Replace("\r", "").Split('\n');
				return lines.Where(t => !string.IsNullOrEmpty(t)).ToList();
			}
			return null;
		}

		private DataBaseTable NodeToDataBaseTable(Node node)
		{
			if (node != null && node.Name == "Table")
			{
				var columns = new List<DataBaseColumn>();
				var fcolumns = new List<ForeignKeyColumn>();
				if (node.Childs != null)
				{
					foreach (var n in node.Childs)
					{
						if (n.Name == "Column")
						{
							var columnName = n.Attributes["name"];
							ColumnType columnType;
							Enum.TryParse(n.Attributes["type"], out columnType);
							if (n.Attributes.Keys.Contains("foreignkey"))
							{
								var foreignkey = n.Attributes["foreignkey"];
								var referenceTable = n.Attributes["reference"];
								fcolumns.Add(new ForeignKeyColumn(columnName, columnType, referenceTable, foreignkey));
							}
							else
							{
								var isPrimaryKey = false;
								if (n.Attributes.Keys.Contains("primarykey"))
									Boolean.TryParse(n.Attributes["primarykey"], out isPrimaryKey);
								columns.Add(new DataBaseColumn(columnName, columnType, isPrimaryKey));
							}
						}
					}
				}
				return new DataBaseTable(node.Attributes["name"], columns, fcolumns);
			}
			return null;
		}
		#endregion

		#region public methods
		public void Insert(ModelBase newModel)
		{
			Insert(newModel, null);
		}

		public void Insert(ModelBase newModel, ModelBase parentModel)
		{
			var table = this.Mapping.GetTableByObject(newModel.GetType());
			var dictionary = this.Mapping.CreateDatabaseDictionary(table, newModel, parentModel);
			this.DataBaseConnection.InsertElement(new SQLDatabase.ElementInsert(table, dictionary));
		}

		public void Insert(ModelBaseTree newModelTree, ModelBase parentModel)
		{
			Insert(newModelTree.Model, parentModel);
			foreach (var modelList in newModelTree.Children)
			{
				foreach (var child in modelList)
				{
					Insert(child, newModelTree.Model);
				}
			}
		}

		public string GetDatabaseName()
		{
			return this.databaseName;
		}
		#endregion
	}
}