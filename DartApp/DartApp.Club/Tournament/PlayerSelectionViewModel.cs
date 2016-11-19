using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Base;
using DartApp.Controls;
using DartApp.QueryService;
using DartApp.Services;

namespace DartApp.Club.Tournament
{
	public class PlayerSelectionViewModel : ViewModelBase
	{
		#region members
		private Models.Tournament tournament;
		private RelayCommand backCommand = null;
		private RelayCommand startCommand = null;
		private int fLCount = 0;
		private int playerCount = 0;
		private int tournamentCount = 0;
		private ItemSelectionViewModel itemSelection = null;
		private ObservableCollection<string> settingSelection = null;
		private string selectedSetting = "";
		private IEventService eventService;
		#endregion

		#region ctors
		public PlayerSelectionViewModel(Models.Tournament tournament, IDartAppQueryService queryService, IEventService eventService)
		{
			this.tournament = tournament;
			this.tournament.Date = DateTime.Today;
			this.eventService = eventService;
			var players = queryService.GetAllPlayers();
			var allPlayers = new List<object>();
			players.ForEach(x => allPlayers.Add(x));
			this.itemSelection = new ItemSelectionViewModel(new ObservableCollection<object>(allPlayers), new ObservableCollection<object>(), "Alle Spieler", "Ausgewählte Spieler");
			this.itemSelection.ItemSelected += PlayerCountIncreased;
			this.itemSelection.ItemDeselected += PlayerCountDecreased;
			UpdateValues();
			this.selectedSetting = this.settingSelection.FirstOrDefault();
		}

		void PlayerCountIncreased(object selectedItem, List<object> selectedItems)
		{
			this.playerCount = selectedItems.Count;
			UpdateValues();
		}

		void PlayerCountDecreased(object selectedItem, List<object> selectedItems)
		{
			this.playerCount--;
			UpdateValues();
		}
		#endregion

		#region properties
		public ICommand BackCommand
		{
			get
			{
				if (this.backCommand == null)
				{
					this.backCommand = new RelayCommand(
						param => Back()
					);
				}
				return this.backCommand;
			}
		}
		public ICommand StartCommand
		{
			get
			{
				if (this.startCommand == null)
				{
					this.startCommand = new RelayCommand(
						param => Start(),
						param => CanStart()
					);
				}
				return this.startCommand;
			}
		}
		public int FLCount
		{
			get
			{
				return this.fLCount;
			}
			set
			{
				this.fLCount = value;
				OnPropertyChanged("FLCount");
			}
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
		public ObservableCollection<string> SettingSelection
		{
			get
			{
				return this.settingSelection;
			}
			set
			{
				this.settingSelection = value;
				OnPropertyChanged("SettingSelection");
			}
		}
		public string SelectedSetting
		{
			get
			{
				return this.selectedSetting;
			}
			set
			{
				this.selectedSetting = value;
				OnPropertyChanged("SelectedSetting");
			}
		}

		public DateTime Date
		{
			get
			{
				return this.tournament.Date;
			}
			set
			{
				this.tournament.Date = value;
				OnPropertyChanged("Date");
			}
		}
		#endregion

		#region private methods
		private void Back()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		private void Start()
		{
			var players = new List<Models.Player>();
			this.ItemSelection.SelectedObjects.ToList().ForEach(x => players.Add((Models.Player)x));
			var eventArgs = new List<object>(){this.tournament, players, this.SelectedSetting};
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Tournament, eventArgs);
		}

		private bool CanStart()
		{
			return this.playerCount > 3;
		}

		private void UpdateValues()
		{
			this.tournamentCount = TournamentController.GetTournamentSize(this.playerCount);
			this.FLCount = this.tournamentCount > 0 ? this.tournamentCount - this.playerCount : 0;
			this.SettingSelection = GetSettingSelection();
		}

		private ObservableCollection<string> GetSettingSelection()
		{
			var ret = new ObservableCollection<string>();
			ret.Add("Keine");
			if (this.playerCount > 3)
			{
				int cnt = 2;
				while (cnt < this.tournamentCount)
				{
					ret.Add("Erste " + cnt);
					cnt = cnt * 2;
				}
				ret.Add("Alle");
			}
			return ret;
		}
		#endregion

		#region public methods
		#endregion
	}
}
