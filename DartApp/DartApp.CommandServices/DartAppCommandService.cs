using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInitializer;
using DartApp.Models;
using SQLDatabase;

namespace DartApp.CommandServices
{
	public class DartAppCommandService : IDartAppCommandService
	{
		private ORDictionary mapping;
		private DataBaseConnection connection;

		public DartAppCommandService()
		{
			var dbCreator = DataBaseCreator.GetInstance();
			if (dbCreator != null)
			{
				this.mapping = dbCreator.Mapping;
				this.connection = dbCreator.DataBaseConnection;
			}
		}

		public void InitializeDatabase(string setup, string mappingPath, string testValueFile)
		{
			var dbCreator = DataBaseCreator.GetInstance(setup, mappingPath, testValueFile);
			this.mapping = dbCreator.Mapping;
			this.connection = dbCreator.DataBaseConnection;
		}

		public void InsertPlayer(Player newPlayer)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, newPlayer);
			DataBaseCreator.GetInstance().DataBaseConnection.InsertElement(new SQLDatabase.ElementInsert(table, dictionary));
		}

		public void UpdatePlayer(Player newPlayer)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var dictionary = this.mapping.CreateDatabaseDictionary(table, newPlayer);
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, newPlayer.GetId()));
			DataBaseCreator.GetInstance().DataBaseConnection.UpdateElement(new SQLDatabase.ElementUpdate(table, dictionary, condition));
		}

		public void DeletePlayer(Player playerToDelete)
		{
			var table = this.mapping.GetTableByObject(typeof(Player));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToDelete.GetId()));
			DataBaseCreator.GetInstance().DataBaseConnection.DeleteElement(new SQLDatabase.ElementDelete(table, condition));
		}


		public void AddToHoliday(Player newPlayer)
		{
			var table = this.mapping.GetTableByObject(typeof(Holiday));
			var holiday = new Holiday(newPlayer);
			var dictionary = this.mapping.CreateDatabaseDictionary(table, holiday);
			DataBaseCreator.GetInstance().DataBaseConnection.InsertElement(new SQLDatabase.ElementInsert(table, dictionary));
		}

		public void RemoveFromHoliday(Player playerToRemove)
		{
			var table = this.mapping.GetTableByObject(typeof(Holiday));
			var condition = new Condition().Add(new PropertyExpression(table.Columns["Pid"], CompareEnum.Equals, playerToRemove.GetId()));
			DataBaseCreator.GetInstance().DataBaseConnection.DeleteElement(new SQLDatabase.ElementDelete(table, condition));
		}
	}
}
