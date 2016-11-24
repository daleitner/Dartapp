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
			this.CreatedAt = DateTime.Now;
			this.Tournaments = new List<Tournament>();
			this.AdditionalColumns = new List<AdditionalColumn>();
			this.RelevantTournaments = 0;
		}

		public TournamentSeries(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.Name = itemArray[1];
			this.CreatedAt = DateTime.Parse(itemArray[2]);
			this.RelevantTournaments = Int32.Parse(itemArray[3]);
			this.Tournaments = new List<Tournament>();
			this.AdditionalColumns = new List<AdditionalColumn>();
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

		public DateTime CreatedAt { get; set; }

		public List<Tournament> Tournaments;

		public List<AdditionalColumn> AdditionalColumns;

		public int RelevantTournaments { get; set; }
	}
}
