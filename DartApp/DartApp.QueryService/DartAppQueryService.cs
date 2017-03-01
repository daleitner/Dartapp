using Base;
using DartApp.Models;
using DataBaseInitializer;
using SQLDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.QueryService
{
	public class DartAppQueryService : IDartAppQueryService
	{
		private DataBaseConnection connection;
		private ORDictionary mapping;
		public DartAppQueryService()
		{
			this.connection = DataBaseManager.GetInstance().DataBaseConnection;
			this.mapping = DataBaseManager.GetInstance().Mapping;
		}

		public List<ModelBase> GetSearchResult(string search, ModelEnum modelType)
		{
			List<ModelBase> ret = new List<ModelBase>();
			switch (modelType)
			{
				case ModelEnum.Player:
					var query = new DataBaseQuery(this.mapping.GetTableByObject(typeof(Player)));
					var result = this.connection.ExecuteQuery(query);
					foreach (var res in result)
					{
						var pl = new Player(res);
						if (String.IsNullOrEmpty(search) || pl.DisplayName.Contains(search))
							ret.Add(pl);
					}
					break;
				case ModelEnum.TournamentSeries:
					var tournamentQuery = new DataBaseQuery(this.mapping.GetTableByObject(typeof(TournamentSeries)));
					var tournamentResult = this.connection.ExecuteQuery(tournamentQuery);
					foreach (var res in tournamentResult)
					{
						var ts = new TournamentSeries(res);
						if (String.IsNullOrEmpty(search) || ts.DisplayName.Contains(search))
						{
							var ttable = this.mapping.GetTableByObject(typeof(Tournament));
							var tcondition = new Condition().Add(new PropertyExpression(ttable.Columns["TournamentSeries"], CompareEnum.Equals, ts.GetId()));
							var tQuery = new DataBaseQuery(ttable, tcondition);
							var tRes = this.connection.ExecuteQuery(tQuery);
							foreach (var tournament in tRes)
							{
								ts.Tournaments.Add(new Tournament(tournament));
							}
							ret.Add(ts);
						}
					}
					break;
			}
			return ret.OrderBy(x => x.DisplayName).ToList();
		}

		public List<Player> GetAllPlayers()
		{
			List<Player> ret = new List<Player>();
			GetSearchResult("", ModelEnum.Player).ForEach(x => ret.Add((Player)x));
			return ret;
		}

		public List<Player> GetAllHolidayPlayers()
		{
		/*	List<Player> ret = new List<Player>();
			GetSearchResult("", ModelEnum.Holiday).ForEach(x => ret.Add((Player)x));
			return ret;*/
			return null;
		}


		public List<PlacementPoint> GetPlacementPoints()
		{
			List<PlacementPoint> ret = new List<PlacementPoint>();
			var query = new DataBaseQuery(this.mapping.GetTableByObject(typeof(PlacementPoint)));
			var result = this.connection.ExecuteQuery(query);
			foreach (var res in result)
			{
				var pl = new PlacementPoint(res);
				ret.Add(pl);
			}
			return ret;
		}

		public List<TournamentSeries> GetTournamentSeries()
		{
			var ret = new List<TournamentSeries>();
			var tournamentQuery = new DataBaseQuery(this.mapping.GetTableByObject(typeof(TournamentSeries)));
			var tournamentResult = this.connection.ExecuteQuery(tournamentQuery);
			foreach (var res in tournamentResult)
			{
				var ts = new TournamentSeries(res);
				ret.Add(ts);
			}
			return ret.OrderByDescending(x => x.CreatedAt).ToList();
		}


		public List<Player> GetSelectedPlayersOrderedByStatistics(List<Player> selectedPlayers, TournamentSeries series)
		{
			List<Statistic> statistics = new List<Statistic>();
			foreach (var player in selectedPlayers)
			{
				var table = this.mapping.GetTableByObject(typeof(Statistic));
				var condition = new Condition()
					.Add(new LogicalExpression(LogicalEnum.AND)
						.Add(new PropertyExpression(table.Columns["Player"], CompareEnum.Equals, player.GetId()))
						.Add(new PropertyExpression(table.Columns["TournamentSeries"], CompareEnum.Equals, series.GetId())));
				var query = new DataBaseQuery(table, condition);
				var res = this.connection.ExecuteQuery(query);
				if (res.Count > 0)
				{
					var statistic = new Statistic(res[0]);
					statistic.Player = player;
					statistic.TournamentSeries = series;
					statistics.Add(statistic);
				}
			}
			statistics = statistics.OrderByDescending(x => x.Points).ThenByDescending(x => (x.WonSets - x.LostSets)).ThenByDescending(x => (x.WonLegs - x.LostLegs)).ToList();
			return statistics.Select(x => x.Player).ToList();
		}


		public TournamentSeries GetTournamentSeriesOfTournament(Tournament tournament)
		{
			var ttable = this.mapping.GetTableByObject(typeof(Tournament));
			var tstable = this.mapping.GetTableByObject(typeof(TournamentSeries));
			var subCondition = new Condition().Add(new PropertyExpression(ttable.Columns["Tid"], CompareEnum.Equals, tournament.GetId()));
			var subQuery = new DataBaseQuery(new List<DataBaseColumn>() { ttable.Columns["TournamentSeries"] }, ttable, subCondition);
			var condition = new Condition().Add(new PropertyExpression(tstable.Columns["Tid"], CompareEnum.In, subQuery));
			var query = new DataBaseQuery(tstable, condition);
			var res = this.connection.ExecuteQuery(query);
			if (res.Count <= 0)
				return null;
			var ret = new TournamentSeries(res.First());
			return ret;
		}

		public TournamentSeries GetFullTournamentSeries(TournamentSeries series)
		{
			series.Tournaments = new List<Tournament>();
			series.AdditionalColumns = new List<AdditionalColumn>();

			var tempPlayers = new List<Player>();

			//get tournaments with matches and players of matches
			var ttable = this.mapping.GetTableByObject(typeof(Tournament));
			var tcondition = new Condition().Add(new PropertyExpression(ttable.Columns["TournamentSeries"], CompareEnum.Equals, series.GetId()));
			var sortDict = new Dictionary<DataBaseColumn, SortEnum> {{ttable.Columns["Key"], SortEnum.ASC}};
			var tQuery = new DataBaseQuery(ttable.Columns.Values.ToList(), ttable, tcondition, sortDict);
			var tRes = this.connection.ExecuteQuery(tQuery);
			foreach (var tournament in tRes)
			{
				var t = new Tournament(tournament);

				var mtable = this.mapping.GetTableByObject(typeof (Match));
				var mcondition =
					new Condition().Add(new PropertyExpression(mtable.Columns["Tid"], CompareEnum.Equals, t.GetId()));
				var msortDict = new Dictionary<DataBaseColumn, SortEnum> { {mtable.Columns["Positionkey"], SortEnum.ASC} };
				var mQuery = new DataBaseQuery(mtable.Columns.Values.ToList(), mtable, mcondition, msortDict);
				var mRes = this.connection.ExecuteQuery(mQuery);
				foreach (var m in mRes)
				{
					var match = new Match(m);
					match.Player1 = GetPlayerById(m[2], tempPlayers);
					match.Player2 = GetPlayerById(m[3], tempPlayers);
					t.Matches.Add(match);
				}

				var ptable = this.mapping.GetTableByObject(typeof(Placement));
				var pcondition =
					new Condition().Add(new PropertyExpression(ptable.Columns["Tid"], CompareEnum.Equals, t.GetId()));
				var psortDict = new Dictionary<DataBaseColumn, SortEnum> { { ptable.Columns["Position"], SortEnum.ASC } };
				var pQuery = new DataBaseQuery(ptable.Columns.Values.ToList(), ptable, pcondition, psortDict);
				var pRes = this.connection.ExecuteQuery(pQuery);
				foreach (var p in pRes)
				{
					var placement = new Placement(p);
					placement.Player = GetPlayerById(p[2], tempPlayers);
					t.Placements.Add(placement);
				}
				series.Tournaments.Add(t);
			}

			//get additional Columns with column Values and Player
			var ctable = this.mapping.GetTableByObject(typeof(AdditionalColumn));
			var ccondition = new Condition().Add(new PropertyExpression(ctable.Columns["Tid"], CompareEnum.Equals, series.GetId()));
			var cQuery = new DataBaseQuery(ctable, ccondition);
			var cRes = this.connection.ExecuteQuery(cQuery);
			foreach (var c in cRes)
			{
				var column = new AdditionalColumn(c);

				var cvtable = this.mapping.GetTableByObject(typeof(AdditionalColumnValue));
				var cvcondition = new Condition().Add(new PropertyExpression(cvtable.Columns["AdditionalColumn"], CompareEnum.Equals, column.GetId()));
				var cvQuery = new DataBaseQuery(cvtable, cvcondition);
				var cvRes = this.connection.ExecuteQuery(cvQuery);
				foreach (var value in cvRes)
				{
					var columnValue = new AdditionalColumnValue(value);
					columnValue.Player = GetPlayerById(value[2], tempPlayers);
					column.Values.Add(columnValue);
				}
				series.AdditionalColumns.Add(column);
			}
			return series;
		}

		public List<Statistic> GetStatisticsByTournamentSeries(TournamentSeries selectedSeries)
		{
			var statistics = new List<Statistic>();
			var stable = this.mapping.GetTableByObject(typeof(Statistic));
			var scondition = new Condition().Add(new PropertyExpression(stable.Columns["TournamentSeries"], CompareEnum.Equals, selectedSeries.GetId()));
			var sQuery = new DataBaseQuery(stable, scondition);
			var sRes = this.connection.ExecuteQuery(sQuery);
			foreach (var result in sRes)
			{
				var statistic = new Statistic(result);
				statistic.Player = GetPlayerById(result[11], new List<Player>());
				statistics.Add(statistic);
			}
			return statistics;
		}

		private Player GetPlayerById(string playerId, List<Player> tempPlayers)
		{
			var ret = tempPlayers.FirstOrDefault(x => x.GetId() == playerId);
			if (ret != null)
				return ret;
			var table = this.mapping.GetTableByObject(typeof (Player));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerId));
			var query = new DataBaseQuery(table, condition);
			var res = this.connection.ExecuteQuery(query).FirstOrDefault();
			if(res == null)
				return new Player("FL");
			tempPlayers.Add(new Player(res));
			return new Player(res);
		}
	}
}
