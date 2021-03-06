﻿using System;
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
		private bool oldMode = false;
		public Tournament()
			:base()
		{
			this.Matches = new List<Match>();
			this.Placements = new List<Placement>();
		}

		public Tournament(List<string> itemArray)
			:this()
		{
			this.Id = itemArray[0];
			this.Key = Int32.Parse(itemArray[1]);
			this.Date = DateTime.Parse(itemArray[2]);
			this.State = (TournamentState)Enum.Parse(typeof(TournamentState), itemArray[3]);
			this.Matches = new List<Match>();
			this.Placements = new List<Placement>();
		}

		public int Key { get; set; }

		public DateTime Date { get; set; }

		public TournamentState State { get; set; }

		public List<Match> Matches { get; set; }

		public List<Placement> Placements { get; set; }

		public List<Player> GetAllPlayers()
		{
			var ret = new List<Player>();
			foreach (var match in this.Matches)
			{
				if(match.Player1 != null && match.Player1.VorName != "FL" && !ret.Contains(match.Player1))
					ret.Add(match.Player1);
				if (match.Player2 != null && match.Player2.VorName != "FL" && !ret.Contains(match.Player2))
					ret.Add(match.Player2);
			}
			return ret;
		}

		public bool IsOldMode()
		{
			return this.oldMode;
		}

		public void SetOldMode()
		{
			this.oldMode = true;
		}
	}
}
