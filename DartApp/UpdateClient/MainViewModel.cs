using Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using DataBaseInitializer;

namespace UpdateClient
{
	public class MainViewModel : ViewModelBase
	{
		#region members

		private readonly string newestVersion = "1.0";
		private readonly DataBaseManager dbCreator;
		private DataTable data = null;
		private RelayCommand updateCommand = null;
		private RelayCommand executeCommand = null;
		private string dataBaseName = "";
		private string version = "";
		private string statement = "";
		#endregion

		#region ctors
		public MainViewModel()
		{
			var setup = Directory.GetCurrentDirectory() + "\\database.xml";
			var testValueFile = Directory.GetCurrentDirectory() + "\\dbtestvalues.txt";
			var mappingPath = Directory.GetCurrentDirectory() + "\\mapping.xml";

			try
			{
				this.dbCreator = DataBaseManager.GetInstance(setup, mappingPath, testValueFile);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
			try
			{
				var res = this.dbCreator.DataBaseConnection.ExecuteQuery("select* from VersionTable;");
				if (res.Count != 1)
					this.Version = "-";
				else
				{
					this.Version = res[0][1];
				}
			}
			catch (NullReferenceException ne)
			{
				this.Version = "-";
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		#endregion

		#region properties
		public DataTable Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
				OnPropertyChanged("Data");
			}
		}
		public ICommand UpdateCommand
		{
			get
			{
				if (this.updateCommand == null)
				{
					this.updateCommand = new RelayCommand(
						param => Update(),
						param => CanUpdate()
					);
				}
				return this.updateCommand;
			}
		}

		public ICommand ExecuteCommand
		{
			get
			{
				if (this.executeCommand == null)
				{
					this.executeCommand = new RelayCommand(
						param => Execute()
					);
				}
				return this.executeCommand;
			}
		}
		public string DataBaseName
		{
			get
			{
				return this.dataBaseName;
			}
			set
			{
				this.dataBaseName = value;
				OnPropertyChanged("DataBaseName");
			}
		}
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
				OnPropertyChanged("Version");
			}
		}
		public string Statement
		{
			get
			{
				return this.statement;
			}
			set
			{
				this.statement = value;
				OnPropertyChanged("Statement");
			}
		}
		#endregion

		#region private methods
		private void Update()
		{
		}

		private bool CanUpdate()
		{
			return this.Version == "-" || System.Version.Parse(this.newestVersion) > System.Version.Parse(this.Version);
		}

		private void Execute()
		{
			try
			{
				var res = this.dbCreator.DataBaseConnection.ExecuteQuery(this.Statement);

				var table = new DataTable();
				var cnt = res[0].Count;
				for (int i = 0; i < cnt; i++)
				{
					table.Columns.Add("Column " + (i + 1));
				}
				foreach (var row in res)
				{
					table.Rows.Add(row.ToArray());
				}
				this.Data = table;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				this.Data = null;
			}
		}

		#endregion

		#region public methods
		#endregion
	}
}
