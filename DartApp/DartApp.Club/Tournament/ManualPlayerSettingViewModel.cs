using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Base;
using DartApp.Models;
using DartApp.Services;

namespace DartApp.Club.Tournament
{
	public class ManualPlayerSettingViewModel : ViewModelBase
	{
		#region members
		private RelayCommand upCommand = null;
		private RelayCommand downCommand = null;
		private RelayCommand backCommand = null;
		private RelayCommand startCommand = null;
		private ObservableCollection<Player> players = null;
		private Player selectedPlayer = null;
		private bool oldMode = false;
		private readonly IEventService eventService;
		private readonly Models.Tournament tournament;
		private readonly TournamentSeries series;
		#endregion

		#region ctors
		public ManualPlayerSettingViewModel(Models.Tournament tournament, List<Player>players, TournamentSeries series, IEventService eventService )
		{
			this.players = new ObservableCollection<Player>(players);
			this.series = series;
			this.tournament = tournament;
			this.eventService = eventService;
		}
		#endregion

		#region properties
		public ICommand UpCommand
		{
			get
			{
				if (this.upCommand == null)
				{
					this.upCommand = new RelayCommand(
						param => Up(),
						param => CanUp()
					);
				}
				return this.upCommand;
			}
		}
		public ICommand DownCommand
		{
			get
			{
				if (this.downCommand == null)
				{
					this.downCommand = new RelayCommand(
						param => Down(),
						param => CanDown()
					);
				}
				return this.downCommand;
			}
		}
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
						param => Start()
					);
				}
				return this.startCommand;
			}
		}
		public ObservableCollection<Player> Players
		{
			get
			{
				return this.players;
			}
			set
			{
				this.players = value;
				OnPropertyChanged("Players");
			}
		}
		public Player SelectedPlayer
		{
			get
			{
				return this.selectedPlayer;
			}
			set
			{
				this.selectedPlayer = value;
				OnPropertyChanged("SelectedPlayer");
			}
		}

		public bool OldMode
		{
			get
			{
				return this.oldMode;
			}
			set
			{
				this.oldMode = value;
				OnPropertyChanged("OldMode");
			}
		}
		#endregion

		#region private methods
		private void Up()
		{
			int oldIndex = GetSelectedPosition();
			this.Players.Move(oldIndex, oldIndex-1);
		}

		private bool CanUp()
		{
			return this.SelectedPlayer != null && !this.SelectedPlayer.Equals(this.Players.First());
		}

		private void Down()
		{
			int oldIndex = GetSelectedPosition();
			this.Players.Move(oldIndex, oldIndex + 1);
		}

		private bool CanDown()
		{
			return this.SelectedPlayer != null && !this.SelectedPlayer.Equals(this.Players.Last());
		}

		private void Back()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		private void Start()
		{
			var eventArgs = new List<object>() {this.tournament, this.Players.ToList(), "Manuell setzen fertig", this.series, this.OldMode};
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Tournament, eventArgs);
		}

		private int GetSelectedPosition()
		{
			for (int i = 0; i < this.Players.Count; i++)
			{
				if (this.SelectedPlayer.Equals(this.Players[i]))
					return i;
			}
			return -1;
		}

		#endregion

		#region public methods
		#endregion
	}
}
