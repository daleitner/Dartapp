using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.QueryService
{
	public interface IDartAppQueryService
	{
		List<ModelBase> GetSearchResult(string search, ModelEnum modelType);
		List<Player> GetAllPlayers();
		List<Player> GetAllHolidayPlayers();
	}
}
