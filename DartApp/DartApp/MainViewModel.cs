using System;
using System.IO;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DataBaseInitializer;
using DartApp.Home;
using DartApp.Factory;
using DartApp.Services;
using System.Collections.Generic;
using DartApp.Models;
using DartApp.Club.Tournament;
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

        void eventService_DisplayChanged(DisplayEnum displayEnum, List<object> eventArgs)
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
				case DisplayEnum.PlayerSelection:
					this.Content = this.factory.GetPlayerSelectionViewModel((Tournament)eventArgs[0], (TournamentSeries)eventArgs[1]);
					break;
				case DisplayEnum.Tournament:
					var tournament = TournamentController.DrawMatches((Tournament)eventArgs[0], (List<Player>)eventArgs[1], eventArgs[2].ToString(), this.factory.GetQueryService());
					this.Content = this.factory.GetTournamentViewModel(tournament, (TournamentSeries)eventArgs[3]);
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
