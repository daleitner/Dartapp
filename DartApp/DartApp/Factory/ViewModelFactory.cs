using DartApp.Club.Menu;
using DartApp.Club.Tournament;
using DartApp.CommandServices;
using DartApp.Database;
using DartApp.Home;
using DartApp.Models;
using DartApp.QueryService;
using DartApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace DartApp.Factory
{
	public class ViewModelFactory : IViewModelFactory
	{
        private IEventService eventService;
		private IDartAppQueryService dartAppQueryService;
		private IDartAppCommandService dartAppCommandService;
		private static ViewModelFactory factory;
		private ViewModelFactory()
		{
            this.eventService = (IEventService)EventService.GetInstance();
			this.dartAppCommandService = new DartAppCommandService();
			this.dartAppQueryService = new DartAppQueryService();
		}

		public static ViewModelFactory GetInstance()
		{
			return factory ?? (factory = new ViewModelFactory());
		}

		public HomeViewModel GetHomeViewModel()
		{
			return new HomeViewModel(this.eventService);
		}


        public DatabaseMainViewModel GetDatabaseMainViewModel()
        {
            return new DatabaseMainViewModel(this.eventService, this.dartAppQueryService, this.dartAppCommandService);
        }

		public MainViewModel GetMainViewModel()
		{
			return new MainViewModel(this.dartAppCommandService);
		}

		public ClubMenuViewModel GetClubMenuViewModel(TournamentSeries selectedSeries)
		{
			return new ClubMenuViewModel(selectedSeries, this.dartAppQueryService, this.eventService);
		}


		public PlayerSelectionViewModel GetPlayerSelectionViewModel(Tournament tournament, TournamentSeries series)
		{
			return new PlayerSelectionViewModel(tournament, series, this.dartAppQueryService, this.eventService);
		}


		public TournamentViewModel GetTournamentViewModel(Tournament tournament, TournamentSeries series)
		{
			return new TournamentViewModel(tournament, series, this.eventService, this.dartAppCommandService);
		}


		public IDartAppQueryService GetQueryService()
		{
			return this.dartAppQueryService;
		}

		public ViewModelBase GetManualPlayerSettingViewModel(Tournament tournament, List<Player> players, TournamentSeries tournamentSeries)
		{
			return new ManualPlayerSettingViewModel(tournament, players, tournamentSeries, eventService);
		}
	}
}
