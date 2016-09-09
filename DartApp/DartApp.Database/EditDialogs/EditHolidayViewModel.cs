using Base;
using DartApp.Controls;
using DartApp.Models;
using DartApp.QueryService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartApp.Database.EditDialogs
{
	public class EditHolidayViewModel : ViewModelBase
	{
		private ItemSelectionViewModel itemSelection;
		private RelayCommand saveCommand = null;
		private RelayCommand cancelCommand = null;
		public delegate void ButtonClickedEventHandler(List<Player> newHolidayPlayers);
		public event ButtonClickedEventHandler ButtonClicked = null;
		public EditHolidayViewModel(IDartAppQueryService queryService)
		{
			List<Player> allPlayers = queryService.GetAllPlayers();
			List<Player> selectedPlayers = queryService.GetAllHolidayPlayers();
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

			this.itemSelection = new ItemSelectionViewModel(allObjects, selectedObjects, "Alle Spieler:", "Holiday-Spieler:");
		}

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
						param => Save()
							);
				}
				return this.saveCommand;
			}
		}

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
				List<Player> players = new List<Player>();
				this.ItemSelection.SelectedObjects.ToList().ForEach(x => players.Add((Player)x));
				ButtonClicked(players);
			}
		}
	}
}
