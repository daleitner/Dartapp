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
			List<Models.Player> orderedPlayers;
			List<int> order;
			if (setOption == "Manuell setzen")
			{
				orderedPlayers = players;
				order = new List<int>();
				for(int i = 0; i<orderedPlayers.Count; i++)
					order.Add(i);
			}
			else
			{
				orderedPlayers = GetOrderedPlayers(tournament, players, setOption, queryService);
				order = GetOrderForMatches(orderedPlayers.Count);
			}
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
			List<int> shuffledIndicess = new List<int>();
			var rnd = new Random();
			while (shuffledIndicess.Count < players.Count)
			{
				var idx = rnd.Next(0, players.Count);
				if (!shuffledIndicess.Contains(idx))
				{
					shuffledIndicess.Add(idx);
				}
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
			bool isWinnerPlayer1 = true; //is winner of this match Player1 of his next match
			bool isLoserPlayer1 = true; //is loser of this match Player1 of his next match
			if (match.PositionKey < numberOfPlayers / 2) //first round
			{
				winPositionKey = (match.PositionKey + numberOfPlayers) / 2; //S1 -> S2
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				losePositionKey = match.PositionKey / 2 + numberOfPlayers * 3 / 4; //S1 -> V1
				isLoserPlayer1 = match.PositionKey % 2 == 0;

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
					if(!tournament.IsOldMode())
						isDesc = !isDesc;
				}
			
				winPositionKey = (match.PositionKey - start) / 2  + start + 3 * areaSize; //Sx -> Sx+1
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				if (isDesc)
					losePositionKey = start + 2 * areaSize + (start + areaSize - match.PositionKey - 1);
				else
					losePositionKey = match.PositionKey + 2 * areaSize;
				isLoserPlayer1 = false;
			}
			else if (IsLoserAgainstLoser(match.PositionKey, numberOfPlayers)) //V(2x+1) -> V(2x+2)
			{
				var start = numberOfPlayers * 3 / 4;
				var areaSize = numberOfPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize))
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
					start = start + areaSize;
				}

				winPositionKey = match.PositionKey + areaSize;
				isWinnerPlayer1 = true;
			}
			else //V(2x) -> V(2x+1)
			{
				var start = numberOfPlayers;
				var areaSize = numberOfPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize) && areaSize > 0)
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
				}
				if (areaSize == 0)
					return ret;
				winPositionKey = start + areaSize + areaSize / 2 + (match.PositionKey - start) / 2;
				isWinnerPlayer1 = match.PositionKey % 2 == 0;
				if (match.PositionKey == numberOfMatches - 2)
					isWinnerPlayer1 = false;
			}

			if (isWinnerPlayer1) //set winner
				tournament.Matches[winPositionKey].Player1 = match.GetWinner();
			else
				tournament.Matches[winPositionKey].Player2 = match.GetWinner();
			ret.Add(winPositionKey);

			if (losePositionKey >= 0) //set loser
			{
				if (isLoserPlayer1)
					tournament.Matches[losePositionKey].Player1 = match.GetLoser();
				else
					tournament.Matches[losePositionKey].Player2 = match.GetLoser();
				ret.Add(losePositionKey);
			}
			return ret;
		}

		public static List<int> UndoMatch(Models.Match match, Models.Tournament tournament)
		{
			var ret = new List<int>();
			var numberOfMatches = tournament.Matches.Count;
			var numberOfPlayers = tournament.Matches.Count / 2 + 1;
			int winPositionKey = -1;
			int losePositionKey = -1;
			bool isWinnerPlayer1 = true; //is winner of this match Player1 of his next match
			bool isLoserPlayer1 = true; //is loser of this match Player1 of his next match
			if (match.PositionKey < numberOfPlayers / 2) //first round
			{
				winPositionKey = (match.PositionKey + numberOfPlayers) / 2; //S1 -> S2
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				losePositionKey = match.PositionKey / 2 + numberOfPlayers * 3 / 4; //S1 -> V1
				isLoserPlayer1 = match.PositionKey % 2 == 0;

			}
			else if (IsWinnerSide(match.PositionKey, numberOfPlayers)) // winner side
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

				winPositionKey = (match.PositionKey - start) / 2 + start + 3 * areaSize; //Sx -> Sx+1
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				if (isDesc)
					losePositionKey = start + 2 * areaSize + (start + areaSize - match.PositionKey - 1);
				else
					losePositionKey = match.PositionKey + 2 * areaSize;
				isLoserPlayer1 = false;
			}
			else if (IsLoserAgainstLoser(match.PositionKey, numberOfPlayers)) //V(2x+1) -> V(2x+2)
			{
				var start = numberOfPlayers * 3 / 4;
				var areaSize = numberOfPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize))
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
					start = start + areaSize;
				}

				winPositionKey = match.PositionKey + areaSize;
				isWinnerPlayer1 = true;
			}
			else //V(2x) -> V(2x+1)
			{
				var start = numberOfPlayers;
				var areaSize = numberOfPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize) && areaSize > 0)
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
				}
				if (areaSize == 0)
					return ret;
				winPositionKey = start + areaSize + areaSize / 2 + (match.PositionKey - start) / 2;
				isWinnerPlayer1 = match.PositionKey % 2 == 0;
				if (match.PositionKey == numberOfMatches - 2)
					isWinnerPlayer1 = false;
			}

			if (isWinnerPlayer1) //set winner
				tournament.Matches[winPositionKey].Player1 = null;
			else
				tournament.Matches[winPositionKey].Player2 = null;
			ret.Add(winPositionKey);

			if (losePositionKey >= 0) //set loser
			{
				if (isLoserPlayer1)
					tournament.Matches[losePositionKey].Player1 = null;
				else
					tournament.Matches[losePositionKey].Player2 = null;
				ret.Add(losePositionKey);
			}
			return ret;
		} 

		private static bool IsLoserAgainstLoser(int positionKey, int numberOfPlayers)
		{
			var start = numberOfPlayers * 3 / 4;
			var area = numberOfPlayers / 4;
			do
			{
				if (positionKey >= start && positionKey < start + area)
					return true;
				start = start + 2 * area;
				area = area / 2;
				start = start + area;
			} while (area > 0);
			return false;
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


		/// <summary>
		/// Auf welchem Platz landet der Verlierer.
		/// </summary>
		/// <param name="match"></param>
		/// <param name="tournament"></param>
		/// <returns>Platz des Verlierers</returns>
		internal static int GetRanking(Models.Match match, Models.Tournament tournament)
		{
			var numberOfPlayers = tournament.Matches.Count / 2 + 1;
			var start = numberOfPlayers * 3 / 4;
			var area = numberOfPlayers / 4;
			var actRanking = numberOfPlayers - area + 1;
			while (area > 0)
			{
				if (match.PositionKey >= start && match.PositionKey < start + area)
				{
					actRanking = actRanking + match.PositionKey - start;
					return actRanking;
				}

				actRanking = actRanking - area;
				start = start + area;

				if (match.PositionKey >= start && match.PositionKey < start + area)
				{
					actRanking = actRanking + match.PositionKey - start;
					return actRanking;
				}
				area = area / 2;
				actRanking = actRanking - area;
				start = start + 3 * area;
			}
			if (match.PositionKey == tournament.Matches.Count - 1) //if Match is Finale
				return 1;
			return 0; //if Match is at winner side
		}
	}
}
