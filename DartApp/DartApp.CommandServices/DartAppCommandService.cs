using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInitializer;

namespace DartApp.CommandServices
{
	public class DartAppCommandService : IDartAppCommandService
	{
		public void InitializeDatabase(string setup, string mappingPath, string testValueFile)
		{
			DataBaseCreator.GetInstance(setup, mappingPath, testValueFile);
		}
	}
}
