using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace DartApp.Models
{
	public enum TournamentState
	{
		Open,
		Ongoing,
		Closed
	}

	public class Tournament : ModelBase
	{
		public Tournament()
			:base()
		{
			this.Matches = new List<Match>();
		}

		public Tournament(List<string> itemArray)
			:this()
		{
			this.Id = itemArray[0];
			this.Date = DateTime.Parse(itemArray[1]);
			this.State = (TournamentState)Enum.Parse(typeof(TournamentState), itemArray[2]);
		}

		public DateTime Date { get; set; }

		public TournamentState State { get; set; }

		public List<Match> Matches { get; set; }
	}
}
