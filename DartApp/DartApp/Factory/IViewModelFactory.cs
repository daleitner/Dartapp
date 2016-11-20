﻿using DartApp.Club.Menu;
using DartApp.Club.Tournament;
using DartApp.Database;
using DartApp.Home;
using DartApp.Models;
using DartApp.QueryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Factory
{
    public interface IViewModelFactory
    {
		MainViewModel GetMainViewModel();
        HomeViewModel GetHomeViewModel();
        DatabaseMainViewModel GetDatabaseMainViewModel();
		ClubMenuViewModel GetClubMenuViewModel();
		PlayerSelectionViewModel GetPlayerSelectionViewModel(Tournament tournament, TournamentSeries series);
		TournamentViewModel GetTournamentViewModel(Tournament tournament, TournamentSeries series);
		IDartAppQueryService GetQueryService();
	}
}
