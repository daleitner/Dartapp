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
			return tournament;
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
