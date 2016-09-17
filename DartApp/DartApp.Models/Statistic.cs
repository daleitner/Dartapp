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
			Player = null;
			TournamentSeries = null;
			WonSets = 0;
			LostSets = 0;
			WonLegs = 0;
			LostLegs = 0;
			FLs = 0;
			Average = 0;
			First = 0;
			Second = 0;
			Third = 0;
			Points = 0;
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
			WonSets = Int32.Parse(itemArray[1]);
			LostSets = Int32.Parse(itemArray[2]);
			WonLegs = Int32.Parse(itemArray[3]);
			LostLegs = Int32.Parse(itemArray[4]);
			FLs = Int32.Parse(itemArray[5]);
			Average = Int32.Parse(itemArray[6]);
			First = Int32.Parse(itemArray[7]);
			Second = Int32.Parse(itemArray[8]);
			Third = Int32.Parse(itemArray[9]);
			Points = Int32.Parse(itemArray[10]);
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