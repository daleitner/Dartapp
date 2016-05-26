using System;
using System.IO;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DataBaseInitializer;
using DartApp.Home;
using DartApp.Factory;
using DartApp.Services;
namespace DartApp
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IDartAppCommandService commandService;
		private ViewModelBase content;
        private IViewModelFactory factory;
        private EventService eventService;
		public MainViewModel(IDartAppCommandService commandService)
		{
            this.eventService = EventService.GetInstance();
            this.eventService.DisplayChanged += eventService_DisplayChanged;
            this.factory = ViewModelFactory.GetInstance();
			this.commandService = commandService;
            this.content = this.factory.GetHomeViewModel();
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

        void eventService_DisplayChanged(DisplayEnum displayEnum)
        {
            switch (displayEnum)
            {
                case DisplayEnum.Home:
                    this.Content = this.factory.GetHomeViewModel();
                    break;
                case DisplayEnum.Database:
                    this.Content = this.factory.GetDatabaseMainViewModel();
                    break;
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
