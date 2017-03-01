using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DartApp.Models
{
	public class Statistic : ModelBase
	{
		#region ctors
		public Statistic()
		{
			this.Player = null;
			this.TournamentSeries = null;
			this.WonSets = 0;
			this.LostSets = 0;
			this.WonLegs = 0;
			this.LostLegs = 0;
			this.FLs = 0;
			this.Average = 0;
			this.First = 0;
			this.Second = 0;
			this.Third = 0;
			this.Points = 0;
		}

		public Statistic(Player player, TournamentSeries tournamentSeries)
			:this()
		{
			this.Player = player;
			this.TournamentSeries = tournamentSeries;
		}

		public Statistic(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.WonSets = Int32.Parse(itemArray[1]);
			this.LostSets = Int32.Parse(itemArray[2]);
			this.WonLegs = Int32.Parse(itemArray[3]);
			this.LostLegs = Int32.Parse(itemArray[4]);
			this.FLs = Int32.Parse(itemArray[5]);
			this.Average = Int32.Parse(itemArray[6]);
			this.First = Int32.Parse(itemArray[7]);
			this.Second = Int32.Parse(itemArray[8]);
			this.Third = Int32.Parse(itemArray[9]);
			this.Points = Int32.Parse(itemArray[10]);
		}
		#endregion

		#region properties
		public Player Player { get; set; }
		public TournamentSeries TournamentSeries { get; set; }
		public int WonSets { get; set; }
		public int LostSets { get; set; }
		public int WonLegs { get; set; }
		public int LostLegs { get; set; }
		public int FLs { get; set; }
		public int Average { get; set; }
		public int First { get; set; }
		public int Second { get; set; }
		public int Third { get; set; }
		public int Points { get; set; }
		#endregion
 }
}