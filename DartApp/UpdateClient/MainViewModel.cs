using Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace UpdateClient
{
	public class MainViewModel : ViewModelBase
	{
		#region members
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
						param => Update()
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

		private void Execute()
		{
		}

		#endregion

		#region public methods
		#endregion
	}
}
