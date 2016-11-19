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
		private ItemSelectionViewModel itemSelection;
		private RelayCommand cancelCommand = null;
		private RelayCommand saveCommand = null;
		private string name = "";
		private int amountTournament = 1;
		public delegate void ButtonClickedEventHandler(TournamentSeries newTournamentSeries);
		public event ButtonClickedEventHandler ButtonClicked = null;
		#endregion

		#region ctors
		public AddTournamentSeriesViewModel(IDartAppQueryService queryService)
		{
			List<Player> allPlayers = queryService.GetAllPlayers();
			List<Player> selectedPlayers = new List<Player>();
			ObservableCollection<object> selectedObjects = new ObservableCollection<object>();
			ObservableCollection<object> allObjects = new ObservableCollection<object>();

			foreach (var x in allPlayers)
			{
				bool check = false;
				foreach (var y in selectedPlayers)
				{
					if (x.GetId() == y.GetId())
						check = true;
				}
				if (!check)
					allObjects.Add(x);
			}

			selectedPlayers.ForEach(x => selectedObjects.Add(x));

			this.itemSelection = new ItemSelectionViewModel(allObjects, selectedObjects, "Alle Spieler:", "Zulässige Spieler:");
		}
		#endregion

		#region properties
		public ItemSelectionViewModel ItemSelection
		{
			get
			{
				return this.itemSelection;
			}
			set
			{
				this.itemSelection = value;
				OnPropertyChanged("ItemSelection");
			}
		}

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
