using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace DartApp.Models
{
	public class TournamentSeries : ModelBase
	{
		private string name = "";
		public TournamentSeries()
			:base()
		{
		}

		public TournamentSeries(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Name = itemArray[1];
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.DisplayName = this.name;
			}
		}
	}
}
