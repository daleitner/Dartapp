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
			//var table = this.mapping.GetTableByObject(typeof(Player));
			//var dictionary = this.mapping.CreateDatabaseDictionary(table, newPlayer);
			//DataBaseManager.GetInstance().DataBaseConnection.InsertElement(new SQLDatabase.ElementInsert(table, dictionary));
		}

		public void UpdatePlayer(Player newPlayer)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, newPlayer);
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, newPlayer.GetId()));
			DataBaseManager.GetInstance().DataBaseConnection.UpdateElement(new SQLDatabase.ElementUpdate(table, dictionary, condition));
		}

		public void DeletePlayer(Player playerToDelete)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToDelete.GetId()));
			DataBaseManager.GetInstance().DataBaseConnection.DeleteElement(new SQLDatabase.ElementDelete(table, condition));
		}


		public void AddToHoliday(Player newPlayer)
		{
			/*var table = this.mapping.GetTableByObject(typeof(Holiday));
			var holiday = new Holiday(newPlayer);
			var dictionary = this.mapping.CreateDatabaseDictionary(table, holiday);
			DataBaseManager.GetInstance().DataBaseConnection.InsertElement(new SQLDatabase.ElementInsert(table, dictionary));*/
		}

		public void RemoveFromHoliday(Player playerToRemove)
		{
			/*var table = this.mapping.GetTableByObject(typeof(Holiday));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToRemove.GetId()));
			DataBaseManager.GetInstance().DataBaseConnection.DeleteElement(new SQLDatabase.ElementDelete(table, condition));*/
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
	}
}
