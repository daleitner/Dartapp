using System;
using System.Collections.Generic;
using System.Windows.Input;
using Base;
using DartApp.Models;

namespace DartApp.Database.EditDialogs
{
	public class AddTournamentSeriesViewModel : ViewModelBase
	{
		#region members
		private RelayCommand cancelCommand = null;
		private RelayCommand saveCommand = null;
		private string name = "";
		public delegate void ButtonClickedEventHandler(TournamentSeries newTournamentSeries);
		public event ButtonClickedEventHandler ButtonClicked = null;
		#endregion

		#region ctors
		public AddTournamentSeriesViewModel()
		{
		}
		#endregion

		#region properties
		public ICommand CancelCommand
		{
			get
			{
				if (this.cancelCommand == null)
				{
					this.cancelCommand = new RelayCommand(
						param => Cancel()
					);
				}
				return this.cancelCommand;
			}
		}
		public ICommand SaveCommand
		{
			get
			{
				if (this.saveCommand == null)
				{
					this.saveCommand = new RelayCommand(
						param => Save()
					);
				}
				return this.saveCommand;
			}
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
				OnPropertyChanged("Name");
			}
		}
		#endregion

		#region private methods
		private void Cancel()
		{
			if (ButtonClicked != null)
			{
				ButtonClicked(null);
			}
		}

		private void Save()
		{
			if (ButtonClicked != null)
			{
				var tournamentSeries = new TournamentSeries {Name = this.Name};
				for (var i = 1; i<=12; i++)
				{
					var tournament = new Tournament {Date = new DateTime(DateTime.Today.Year, i, 1), State=TournamentState.Open};
					tournamentSeries.Tournaments.Add(tournament);
				}
				ButtonClicked(tournamentSeries);
			}
		}

		#endregion
	}
}
