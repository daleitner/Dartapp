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
	}
}
