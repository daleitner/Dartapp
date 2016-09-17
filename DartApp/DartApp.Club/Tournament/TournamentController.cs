using DartApp.QueryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Tournament
{
	public static class TournamentController
	{
		public static Models.Tournament DrawMatches(Models.Tournament tournament, List<Models.Player> players, string setOption, IDartAppQueryService queryService)
		{
			var orderedPlayers = GetOrderedPlayers(tournament, players, setOption, queryService);
			return tournament;
		}

		private static List<Models.Player> GetOrderedPlayers(Models.Tournament tournament, List<Models.Player> players, string setOption, IDartAppQueryService queryService)
		{
			List<Models.Player> orderedPlayers = new List<Models.Player>();
			if (setOption != "Keine")
			{
				var tournamentSeries = queryService.GetTournamentSeriesOfTournament(tournament);
				var orderedStatisticPlayers = queryService.GetSelectedPlayersOrderedByStatistics(players, tournamentSeries);
				if (setOption != "Alle")
				{
					int playersToSet = Int32.Parse(setOption.Split(' ')[1]);
					orderedPlayers = orderedStatisticPlayers.Take(playersToSet).ToList();
					orderedPlayers.AddRange(ShufflePlayers(orderedStatisticPlayers.Skip(playersToSet).ToList()));
				}
				else
				{
					orderedPlayers = orderedStatisticPlayers;
				}
			}
			else
			{
				orderedPlayers = ShufflePlayers(players);
			}
			var size = GetTournamentSize(orderedPlayers.Count);
			while (orderedPlayers.Count < size)
				orderedPlayers.Add(new Models.Player("FL"));
			return orderedPlayers;
		}

		private static List<Models.Player> ShufflePlayers(List<Models.Player> players)
		{
			List<int> indicess = new List<int>();
			for (int i = 0; i < players.Count; i++)
			{
				indicess.Add(i);
			}

			List<int> shuffledIndicess = new List<int>();
			while (indicess.Count > 0)
			{
				var rnd = new Random().Next(indicess.Count);
				shuffledIndicess.Add(indicess[rnd]);
				indicess.RemoveAt(rnd);
			}
			List<Models.Player> ret = new List<Models.Player>();
			for (int i = 0; i < shuffledIndicess.Count; i++)
			{
				ret.Add(players[shuffledIndicess[i]]);
			}
			return ret;
		}

		public static int GetTournamentSize(int players)
		{
			int ret = 8;
			if (players == 0)
				return 0;
			while (ret < players)
				ret = ret * 2;
			return ret;
		}
	}
}
