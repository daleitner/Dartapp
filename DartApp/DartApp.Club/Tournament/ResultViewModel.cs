using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Tournament
{
	public class ResultViewModel : ViewModelBase
	{
		private Models.Match match = null;
		public ResultViewModel(Models.Match match)
		{
			this.match = match;
		}

		public string Player1Name
		{
			get
			{
				return (this.match.Player1 == null ? "" : this.match.Player1.VorName);
			}
			set
			{
				this.match.Player1.VorName = value;
				OnPropertyChanged("Player1Name");
			}
		}

		public string Player2Name
		{
			get
			{
				return (this.match.Player2 == null ? "" : this.match.Player2.VorName);
			}
			set
			{
				this.match.Player2.VorName = value;
				OnPropertyChanged("Player2Name");
			}
		}

		public int Player1Legs
		{
			get
			{
				return this.match.Player1Legs;
			}
			set
			{
				this.match.Player1Legs = value;
				OnPropertyChanged("Player1Legs");
			}
		}

		public int Player2Legs
		{
			get
			{
				return this.match.Player2Legs;
			}
			set
			{
				this.match.Player2Legs = value;
				OnPropertyChanged("Player2Legs");
			}
		}

		public int GetMatchKey()
		{
			return this.match.PositionKey;
		}

		public void Refresh()
		{
			OnPropertyChanged("Player1Name");
			OnPropertyChanged("Player2Name");
			OnPropertyChanged("Player1Legs");
			OnPropertyChanged("Player2Legs");
		}
	}
}
