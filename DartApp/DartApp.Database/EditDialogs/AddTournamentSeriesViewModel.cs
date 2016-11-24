using System;
using System.Collections.Generic;
using System.Windows.Input;
using Base;
using DartApp.Models;
using DartApp.Controls;
using DartApp.QueryService;
using System.Collections.ObjectModel;
using System.Linq;

namespace DartApp.Database.EditDialogs
{
	public class AddTournamentSeriesViewModel : ViewModelBase
	{
		#region members
		private RelayCommand cancelCommand = null;
		private RelayCommand saveCommand = null;
		private string name = "";
		private int amountTournament = 1;
		private int amountRelevantTournaments = 1;
		private ObservableCollection<AdditionalColumn> columns = null; 
		public delegate void ButtonClickedEventHandler(TournamentSeries newTournamentSeries);
		public event ButtonClickedEventHandler ButtonClicked = null;
		#endregion

		#region ctors

		public AddTournamentSeriesViewModel(IDartAppQueryService queryService)
		{
			this.columns = new ObservableCollection<AdditionalColumn>();
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
						param => Save(),
						param => CanSave()
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

		public ObservableCollection<AdditionalColumn> Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				this.columns = value;
				OnPropertyChanged("Columns");
			}
		} 

		public string AmountTournament
		{
			get
			{
				return this.amountTournament.ToString();
			}
			set
			{
				if (!Int32.TryParse(value, out this.amountTournament))
					this.amountTournament = 0;
				OnPropertyChanged("AmountTournament");
			}
		}

		public string AmountRelevantTournaments
		{
			get
			{
				return this.amountRelevantTournaments.ToString();
			}
			set
			{
				if (!Int32.TryParse(value, out this.amountRelevantTournaments))
					this.amountRelevantTournaments = 0;
				OnPropertyChanged("AmountRelevantTournaments");
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
				var tournamentSeries = new TournamentSeries {Name = this.Name, AdditionalColumns = this.Columns.ToList(), RelevantTournaments = this.amountRelevantTournaments};
				for (var i = 1; i<=this.amountTournament; i++)
				{
					var tournament = new Tournament {Date = DateTime.Today, Key = i, State=TournamentState.Open};
					tournamentSeries.Tournaments.Add(tournament);
				}
				ButtonClicked(tournamentSeries);
			}
		}

		private bool CanSave()
		{
			return this.amountTournament > 0;
		}
		#endregion
	}
}
