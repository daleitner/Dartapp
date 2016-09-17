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
			var order = GetOrderForMatches(orderedPlayers.Count);
			for (int i = 0; i < orderedPlayers.Count; i = i + 2)
			{
				var match = new Models.Match(i/2, orderedPlayers[order[i]], orderedPlayers[order[i + 1]]);
				tournament.Matches.Add(match);
			}

			var numberOfMatches = (orderedPlayers.Count - 1) * 2;
			for (int i = tournament.Matches.Count; i < numberOfMatches; i++)
			{
				var match = new Models.Match(i, null, null);
				tournament.Matches.Add(match);
			}
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

		private static List<int> GetOrderForMatches(int playerCount)
		{
			var order = new List<int>(){0,1};
			var actCnt = 2;
			while (actCnt < playerCount)
			{
				actCnt = actCnt * 2;
				for (int i = 0; i < actCnt / 4; i++)
				{
					int pos = 4 * i + 1;
					order.Insert(pos, actCnt - order[pos - 1] - 1);
					order.Insert(pos + 1, actCnt - order[pos + 1] - 1);
				}

			}
			return order;
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

		public static List<int> EndMatch(Models.Match match, Models.Tournament tournament)
		{
			var ret = new List<int>();
			var numberOfMatches = tournament.Matches.Count;
			var numberOfPlayers = tournament.Matches.Count / 2 + 1;
			int winPositionKey = -1;
			int losePositionKey = -1;
			if (match.PositionKey < numberOfPlayers / 2) //first round
			{
				winPositionKey = (match.PositionKey + numberOfPlayers) / 2; //S1 -> S2

				losePositionKey = match.PositionKey / 2 + numberOfPlayers * 3 / 4; //S1 -> V1

			}
			else if(IsWinnerSide(match.PositionKey, numberOfPlayers)) // winner side
			{
				var start = numberOfPlayers / 2;
				var areaSize = numberOfPlayers / 4;
				var isDesc = true;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize))
				{
					start = start + 3 * areaSize;
					areaSize = areaSize / 2;
					isDesc = !isDesc;
				}
			
				winPositionKey = (match.PositionKey - start) / 2  + start + 3 * areaSize; //Sx -> Sx+1
				if (isDesc)
					losePositionKey = start + 2 * areaSize + (start + areaSize - match.PositionKey - 1);
				else
					losePositionKey = match.PositionKey + 2 * areaSize;
			}

			if (match.PositionKey % 2 == 0) //set winner
				tournament.Matches[winPositionKey].Player1 = match.GetWinner();
			else
				tournament.Matches[winPositionKey].Player2 = match.GetWinner();
			ret.Add(winPositionKey);

			if (losePositionKey >= 0) //set loser
			{ //wrong!!!!
				if (match.PositionKey % 2 == 0)
					tournament.Matches[losePositionKey].Player1 = match.GetLoser();
				else
					tournament.Matches[losePositionKey].Player2 = match.GetLoser();
				ret.Add(losePositionKey);
			}
			return ret;
		}

		private static bool IsWinnerSide(int positionKey, int numberOfPlayers)
		{
			var start = numberOfPlayers / 2;
			var areaSize = numberOfPlayers / 4;
			do
			{
				if (positionKey >= start && positionKey < start + areaSize)
					return true;
				start = start + 3 * areaSize;
				areaSize = areaSize / 2;
			} while (areaSize > 0);
			return false;
		}
	}
}
