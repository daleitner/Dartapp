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
				case ModelEnum.Holiday:
					var playertable = this.mapping.GetTableByObject(typeof(Player));
					var holidayTable = this.mapping.GetTableByRelation("Holiday");
					var condition = new Condition()
						.Add(new PropertyExpression(playertable.Columns["Pid"], CompareEnum.In, new DataBaseQuery(new List<DataBaseColumn>(){holidayTable.Columns["Pid"]}, holidayTable, null)));
					var holidayQuery = new DataBaseQuery(playertable, condition);
					var holidayResult = this.connection.ExecuteQuery(holidayQuery);
					foreach (var res in holidayResult)
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
			List<Player> ret = new List<Player>();
			GetSearchResult("", ModelEnum.Holiday).ForEach(x => ret.Add((Player)x));
			return ret;
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
					var ttable = this.mapping.GetTableByObject(typeof(Tournament));
					var tcondition = new Condition().Add(new PropertyExpression(ttable.Columns["TournamentSeries"], CompareEnum.Equals, ts.GetId()));
					var sortDict = new Dictionary<DataBaseColumn, SortEnum>();
					sortDict.Add(ttable.Columns["Tournamentdate"], SortEnum.ASC);
					var tQuery = new DataBaseQuery(ttable.Columns.Values.ToList(), ttable, tcondition, sortDict);
					var tRes = this.connection.ExecuteQuery(tQuery);
					foreach (var tournament in tRes)
					{
						ts.Tournaments.Add(new Tournament(tournament));
					}
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
	}
}
