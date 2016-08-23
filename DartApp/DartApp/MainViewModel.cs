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
			this.commandService = commandService;

			this.factory = ViewModelFactory.GetInstance();
			this.content = this.factory.GetHomeViewModel();
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
				case DisplayEnum.Club:
					this.Content = this.factory.GetClubMenuViewModel();
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
