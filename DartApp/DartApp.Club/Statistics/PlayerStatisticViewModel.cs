using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DartApp.Club.Statistics
{
	public class PlayerStatisticViewModel : ViewModelBase
	{
		#region members
		#endregion

		#region ctors
		public PlayerStatisticViewModel(Statistic playerStatistic)
		{
			this.statistic = playerStatistic;
		}
		#endregion

		#region properties
		public Statistic statistic { get; private set; }
		//public int Ranking
		//{
		//	get
		//	{
		//		return this.statistic.Ranking;
		//	}
		//	set
		//	{
		//		this.ranking = value;
		//		OnPropertyChanged("Ranking");
		//	}
		//}
		public int Points
		{
			get
			{
				return this.statistic.Points;
			}
			set
			{
				this.statistic.Points = value;
				OnPropertyChanged("Points");
			}
		}
		public int WonSets
		{
			get
			{
				return this.statistic.WonSets;
			}
			set
			{
				this.statistic.WonSets = value;
				OnPropertyChanged("WonSets");
			}
		}
		public int LostSets
		{
			get
			{
				return this.statistic.LostSets;
			}
			set
			{
				this.statistic.LostSets = value;
				OnPropertyChanged("LostSets");
			}
		}
		public int SetDifference
		{
			get
			{
				return this.statistic.WonSets - this.statistic.LostSets;
			}
			set
			{
				OnPropertyChanged("SetDifference");
			}
		}
		public int WonLegs
		{
			get
			{
				return this.statistic.WonLegs;
			}
			set
			{
				this.statistic.WonLegs = value;
				OnPropertyChanged("WonLegs");
			}
		}
		public int LostLegs
		{
			get
			{
				return this.statistic.LostLegs;
			}
			set
			{
				this.statistic.LostLegs = value;
				OnPropertyChanged("LostLegs");
			}
		}
		public int LegDifference
		{
			get
			{
				return this.statistic.WonLegs - this.statistic.LostLegs;
			}
			set
			{
				OnPropertyChanged("LegDifference");
			}
		}
		public int FLs
		{
			get
			{
				return this.statistic.FLs;
			}
			set
			{
				this.statistic.FLs = value;
				OnPropertyChanged("FLs");
			}
		}
		public int First
		{
			get
			{
				return this.statistic.First;
			}
			set
			{
				this.statistic.First = value;
				OnPropertyChanged("First");
			}
		}
		public int Second
		{
			get
			{
				return this.statistic.Second;
			}
			set
			{
				this.statistic.Second = value;
				OnPropertyChanged("Second");
			}
		}
		public int Third
		{
			get
			{
				return this.statistic.Third;
			}
			set
			{
				this.statistic.Third = value;
				OnPropertyChanged("Third");
			}
		}
		public string Name
		{
			get
			{
				return this.statistic.Player.ToString();
			}
			set
			{
				OnPropertyChanged("Name");
			}
		}
		#endregion

		#region private methods
		#endregion

		#region public methods
		public override string ToString()
		{
			return this.Name;
		}
		#endregion
	}
}
