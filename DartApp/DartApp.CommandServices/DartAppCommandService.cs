using System.Collections.Generic;
using System.Linq;
using Base;
using DartApp.Models;
using DataBaseInitializer;
using SQLDatabase;

namespace DartApp.CommandServices
{
	public class DartAppCommandService : IDartAppCommandService
	{
		private DataBaseManager dbManager;
		private ORDictionary mapping;
		private DataBaseConnection connection;

		public DartAppCommandService()
		{
			this.dbManager = DataBaseManager.GetInstance();
			if (this.dbManager != null)
			{
				this.mapping = this.dbManager.Mapping;
				this.connection = this.dbManager.DataBaseConnection;
			}
		}

		public void InsertPlayer(Player newPlayer)
		{
			this.dbManager.Insert(newPlayer);
		}

		public void UpdatePlayer(Player newPlayer)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, newPlayer);
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, newPlayer.GetId()));
			this.dbManager.DataBaseConnection.UpdateElement(new ElementUpdate(table, dictionary, condition));
		}

		public void DeletePlayer(Player playerToDelete)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToDelete.GetId()));
			this.dbManager.DataBaseConnection.DeleteElement(new ElementDelete(table, condition));
		}

		public void InsertTournamentSeries(TournamentSeries newTournamentSeries)
		{
			var tournaments = new List<ModelBase>();
			var columns = new List<ModelBase>();
			newTournamentSeries.Tournaments.ForEach(x => tournaments.Add(x));
			newTournamentSeries.AdditionalColumns.ForEach(x => columns.Add(x));
			var tree = new ModelBaseTree(newTournamentSeries, new List<List<ModelBase>>{ tournaments, columns});

			this.dbManager.Insert(tree, null);
		}


		public void InsertStatistic(Statistic stat)
		{
			this.dbManager.Insert(stat);
		}

		public void SaveTournament(Tournament tournament, TournamentSeries series)
		{
			UpdateTourmanent(tournament, series);
			tournament.Matches.ForEach(match => this.dbManager.Insert(match, tournament));
			tournament.Placements.Where(x => x.Player.VorName != "FL").ToList()
				.ForEach(x => this.dbManager.Insert(x, tournament));
		}

		public void SaveAdditionalColumnValues(List<AdditionalColumnValue> columnValues)
		{
			var table = this.mapping.GetTableByObject(typeof (AdditionalColumnValue));

			columnValues.ForEach(x =>
			{
				//check if Player already has a value
				var condition = new Condition().Add(new LogicalExpression(LogicalEnum.AND)
					.Add(new PropertyExpression(table.Columns["Player"], CompareEnum.Equals, x.Player.GetId()))
					.Add(new PropertyExpression(table.Columns["AdditionalColumn"], CompareEnum.Equals, x.Column.GetId())));
				var query = new DataBaseQuery(table, condition);
				var result = this.connection.ExecuteQuery(query).FirstOrDefault();
				if(result == null)
					this.dbManager.Insert(x, x.Column);
				else
				{
					var oldValue = new AdditionalColumnValue(result);
					var vdict = this.mapping.CreateDatabaseDictionary(table, x, x.Column);
					vdict[table.Columns["Aid"]] = oldValue.GetId();
					var vcondition = new Condition().Add(new PropertyExpression(table.Columns["Aid"], CompareEnum.Equals, oldValue.GetId()));

					switch (x.Column.Behavior)
					{
						case BehaviorEnum.Maximum:
							if (x.Value > oldValue.Value)
							{
								this.connection.UpdateElement(new ElementUpdate(table, vdict, vcondition));
							}
							break;
						case BehaviorEnum.Minimum:
							if (x.Value < oldValue.Value)
							{
								this.connection.UpdateElement(new ElementUpdate(table, vdict, vcondition));
							}
							break; 
						case BehaviorEnum.Summe:
							x.Value += oldValue.Value;
							vdict = this.mapping.CreateDatabaseDictionary(table, x, x.Column);
							vdict[table.Columns["Aid"]] = oldValue.GetId();
							this.connection.UpdateElement(new ElementUpdate(table, vdict, vcondition));
						break;
					}
				}
			});
		}

		private void UpdateTourmanent(Tournament tournament, TournamentSeries series)
		{
			var table = this.mapping.GetTableByObject(typeof(Tournament));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, tournament, series);
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Tid"], CompareEnum.Equals, tournament.GetId()));
			this.connection.UpdateElement(new ElementUpdate(table, dictionary, condition));
		}
	}
}
