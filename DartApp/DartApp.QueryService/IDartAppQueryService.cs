using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.QueryService
{
	public interface IDartAppQueryService
	{
		List<ModelBase> GetSearchResult(string search, ModelEnum modelType);
		List<Player> GetAllPlayers();
		//List<Player> GetAllHolidayPlayers();
		List<PlacementPoint> GetPlacementPoints();
		List<TournamentSeries> GetTournamentSeries();
		List<Player> GetSelectedPlayersOrderedByStatistics(List<Player> selectedPlayers, TournamentSeries series);
		TournamentSeries GetTournamentSeriesOfTournament(Tournament tournament);
		TournamentSeries GetFullTournamentSeries(TournamentSeries selectedSeries);
	}
}
