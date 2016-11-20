using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInitializer;
using DartApp.Models;
using SQLDatabase;
using Base;

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
			this.dbManager.DataBaseConnection.UpdateElement(new SQLDatabase.ElementUpdate(table, dictionary, condition));
		}

		public void DeletePlayer(Player playerToDelete)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToDelete.GetId()));
			this.dbManager.DataBaseConnection.DeleteElement(new SQLDatabase.ElementDelete(table, condition));
		}

		public void InsertTournamentSeries(TournamentSeries newTournamentSeries)
		{
			var tournaments = new List<ModelBase>();
			newTournamentSeries.Tournaments.ForEach(x => tournaments.Add(x));
			var tree = new ModelBaseTree(newTournamentSeries, tournaments);

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

		private void UpdateTourmanent(Tournament tournament, TournamentSeries series)
		{
			var table = this.mapping.GetTableByObject(typeof(Tournament));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, tournament, series);
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Tid"], CompareEnum.Equals, tournament.GetId()));
			this.dbManager.DataBaseConnection.UpdateElement(new SQLDatabase.ElementUpdate(table, dictionary, condition));
		}
	}
}
