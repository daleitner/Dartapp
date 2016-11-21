using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.CommandServices
{
    public interface IDartAppCommandService
    {
		void InsertPlayer(Player newPlayer);
		void UpdatePlayer(Player newPlayer);
		void DeletePlayer(Player playerToDelete);
		void InsertTournamentSeries(TournamentSeries newTournamentSeries);
		void InsertStatistic(Statistic stat);
	    void SaveTournament(Tournament tournament, TournamentSeries series);
	    void SaveAdditionalColumnValues(List<AdditionalColumnValue> columnValues);
    }
}
