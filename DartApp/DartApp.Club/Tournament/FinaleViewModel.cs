using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DartApp.Club.Tournament
{
	public class FinaleViewModel : ViewModelBase
	{
		private ResultViewModel result1;
		private ResultViewModel result2;
		private Visibility secondResultVisibility = Visibility.Collapsed;

		public FinaleViewModel(ResultViewModel result)
		{
			this.result1 = result;
		}

		public string Player1Name
		{
			get { return this.result1.Player1Name; }
			set
			{
				this.result1.Player1Name = value;
				OnPropertyChanged("Player1Name");
			}
		}

		public string Player2Name
		{
			get { return this.result1.Player2Name; }
			set
			{
				this.result1.Player2Name = value;
				OnPropertyChanged("Player2Name");
			}
		}

		public int Player1Legs1
		{
			get
			{
				return this.result1.Player1Legs;
			}
			set
			{
				this.result1.Player1Legs = value;
				OnPropertyChanged("Player1Legs1");
			}
		}

		public int Player2Legs1
		{
			get
			{
				return this.result1.Player2Legs;
			}
			set
			{
				this.result1.Player2Legs = value;
				OnPropertyChanged("Player2Legs1");
			}
		}

		public int Player1Legs2
		{
			get
			{
				if (this.result2 == null)
					return 0;
				return this.result2.Player1Legs;
			}
			set
			{
				this.result2.Player1Legs = value;
				OnPropertyChanged("Player1Legs2");
			}
		}

		public int Player2Legs2
		{
			get
			{
				if (this.result2 == null)
					return 0;
				return this.result2.Player2Legs;
			}
			set
			{
				this.result2.Player2Legs = value;
				OnPropertyChanged("Player2Legs2");
			}
		}

		public Visibility SecondResultVisibility
		{
			get { return this.secondResultVisibility; }
			set
			{
				this.secondResultVisibility = value;
				OnPropertyChanged("SecondResultVisibility");
			}
		}

		public int GetMatchKey()
		{
			return this.result1.GetMatchKey();
		}

		public void Refresh()
		{
			OnPropertyChanged("Player1Name");
			OnPropertyChanged("Player2Name");
			OnPropertyChanged("Player1Legs1");
			OnPropertyChanged("Player2Legs1");
			OnPropertyChanged("Player1Legs2");
			OnPropertyChanged("Player2Legs2");
		}

		public void AddSecondResult(ResultViewModel secondResult)
		{
			this.result2 = secondResult;
			this.SecondResultVisibility = Visibility.Visible;
		}

		public void DeleteSecondResult()
		{
			this.result2 = null;
			this.SecondResultVisibility = Visibility.Collapsed;
		}
	}
}