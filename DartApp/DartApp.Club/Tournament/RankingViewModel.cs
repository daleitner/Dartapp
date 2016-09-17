using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DartApp.Club.Tournament
{
	public class RankingViewModel : ViewModelBase
	{
		#region members
		private int ranking = 0;
		private Player player = null;
		#endregion

		#region ctors
		public RankingViewModel(int ranking, Player player)
		{
			this.ranking = ranking;
			this.player = player;
		}
		#endregion

		#region properties
		public int Ranking
		{
			get
			{
				return this.ranking;
			}
			set
			{
				this.ranking = value;
				OnPropertyChanged("Ranking");
			}
		}

		public string Name
		{
			get
			{
				return (this.player == null ? "" : this.player.VorName);
			}
			set
			{
				this.player.VorName = value;
				OnPropertyChanged("Name");
			}
		}
		#endregion
	}
}
