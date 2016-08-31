using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Base;
using DartApp.Models;

namespace DartApp.Database.ModelViews
{
	public class TournamentSeriesViewModel : ViewModelBase
	{
		#region members
		private ObservableCollection<string> tournaments = null;
		#endregion

		#region ctors
		public TournamentSeriesViewModel(TournamentSeries tournamentSeries)
		{
			//this.tournaments = tournamentSeries.Tournaments;
		}
		#endregion

		#region properties
		public ObservableCollection<string> Tournaments
		{
			get
			{
				return this.tournaments;
			}
			set
			{
				this.tournaments = value;
				OnPropertyChanged("Tournaments");
			}
		}
		#endregion

		#region private methods
		#endregion

		#region public methods
		#endregion
	}
}
