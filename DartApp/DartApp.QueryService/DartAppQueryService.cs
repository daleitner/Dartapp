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
			this.connection = DataBaseCreator.GetInstance().DataBaseConnection;
			this.mapping = DataBaseCreator.GetInstance().Mapping;
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

		public List<TournamentSeries> GetSaisons()
		{
			throw new NotImplementedException();
		}
	}
}
