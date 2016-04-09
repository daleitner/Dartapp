using System;
using System.IO;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DataBaseInitializer;
namespace DartApp
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IDartAppCommandService commandService;
		private ViewModelBase content;
		public MainViewModel(IDartAppCommandService commandService)
		{
			this.commandService = commandService;
			this.content = new HomeViewModel();
			var setup = Directory.GetCurrentDirectory() + "\\database.xml";
			var testValueFile = Directory.GetCurrentDirectory() + "\\dbtestvalues.txt";
			var mappingPath = Directory.GetCurrentDirectory() + "\\mapping.xml";
			try
			{
				this.commandService.InitializeDatabase(setup, mappingPath, testValueFile);
				//var dbc = DataBaseCreator.GetInstance(setup, mappingPath, testValueFile);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public ViewModelBase Content
		{
			get
			{
				return this.content;
				
			}
			set
			{
				this.content = value;
				OnPropertyChanged("Content");
			}
		}
	}
}
