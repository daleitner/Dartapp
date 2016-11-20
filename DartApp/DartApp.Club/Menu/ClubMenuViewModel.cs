using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Markup;
using Base;
using DartApp.Services;
using DartApp.QueryService;
using DartApp.Models;

namespace DartApp.Club.Menu
{
	public class ClubMenuViewModel : ViewModelBase
	{
		#region members
		private RelayCommand startCommand = null;
		private RelayCommand statisticsCommand = null;
		private RelayCommand printCommand = null;
		private RelayCommand homeButtonCommand = null;
		private DataTable data = null;
		private List<DataViewModel> dataViewModels = null; 
		private List<PlacementPoint> points = null;
		private List<TournamentSeries> series = null;
		private TournamentSeries selectedSeries = null;
		private string startText = "";
		private string header = "";
		private int actualTournamentIndex = -1;
		private IEventService eventService;
		private IDartAppQueryService queryService;
		#endregion

		#region ctors
		public ClubMenuViewModel(IDartAppQueryService queryService, IEventService eventService)
		{
			this.eventService = eventService;
			this.queryService = queryService;
			LoadData();
		}
		#endregion

		#region properties
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
		public ICommand StatisticsCommand
		{
			get
			{
				if (this.statisticsCommand == null)
				{
					this.statisticsCommand = new RelayCommand(
						param => Statistics()
					);
				}
				return this.statisticsCommand;
			}
		}
		public ICommand PrintCommand
		{
			get
			{
				if (this.printCommand == null)
				{
					this.printCommand = new RelayCommand(
						param => Print()
					);
				}
				return this.printCommand;
			}
		}
		public ICommand HomeButtonCommand
		{
			get
			{
				if (this.homeButtonCommand == null)
				{
					this.homeButtonCommand = new RelayCommand(
						param => HomeButton()
					);
				}
				return this.homeButtonCommand;
			}
		}

		public DataTable Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
				OnPropertyChanged("Data");
			}
		}

		public List<PlacementPoint> Points
		{
			get
			{
				return this.points;
			}
			set
			{
				this.points = value;
				OnPropertyChanged("Points");
			}
		}
		public List<TournamentSeries> Series
		{
			get
			{
				return this.series;
			}
			set
			{
				this.series = value;
				OnPropertyChanged("Series");
			}
		}
		public TournamentSeries SelectedSeries
		{
			get
			{
				return this.selectedSeries;
			}
			set
			{
				this.selectedSeries = value;
				OnPropertyChanged("SelectedSeries");
			}
		}
		public string StartText
		{
			get
			{
				return this.startText;
			}
			set
			{
				this.startText = value;
				OnPropertyChanged("StartText");
			}
		}
		public string Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
				OnPropertyChanged("Header");
			}
		}
		#endregion

		#region private methods

		private void LoadData()
		{
			this.points = this.queryService.GetPlacementPoints();
			this.series = this.queryService.GetTournamentSeries();
			this.selectedSeries = this.series.FirstOrDefault();
			if (this.selectedSeries != null)
			{
				this.selectedSeries.Tournaments = this.queryService.GetFullTournamentsOfSeries(this.selectedSeries);
				for (int i = 0; i < this.selectedSeries.Tournaments.Count; i++)
				{
					if (this.selectedSeries.Tournaments[i].State != TournamentState.Closed)
					{
						this.actualTournamentIndex = i;
						break;
					}
				}
				UpdateData();
				if (this.actualTournamentIndex >= 0)
					this.startText = "Turnier " + this.selectedSeries.Tournaments[this.actualTournamentIndex].Key + " starten";
			}
		}

		private void UpdateData()
		{
			var table = new DataTable();
			table.Columns.Add("Platz");
			table.Columns.Add("Name");
			this.selectedSeries.Tournaments.ForEach(x => table.Columns.Add(x.Key.ToString()));
			table.Columns.Add("Gesamt");
			var allPlayers = GetAllPlayersOfTournamentSeries(this.selectedSeries);
			this.dataViewModels = new List<DataViewModel>();
			foreach (var player in allPlayers)
			{
				var dvm = new DataViewModel {Name = player.VorName + " " + player.NachName};
				foreach (var tournament in this.selectedSeries.Tournaments)
				{
					if (tournament.State == TournamentState.Closed)
					{
						bool found = false;
						foreach (var placement in tournament.Placements)
						{
							if (placement.Player.Equals(player))
							{
								var tmp = placement.Position;
								while (this.Points.FirstOrDefault(x => x.Position == tmp) == null)
									tmp--;
								var p = this.Points.First(x => x.Position == tmp).Points;
								dvm.Columns.Add(tournament.Key.ToString(), p);
								found = true;
								break;
							}
						}
						if(!found)
							dvm.Columns.Add(tournament.Key.ToString(), "");
					}
					else
					{
						dvm.Columns.Add(tournament.Key.ToString(), "");
					}
				}
				var sum = 0;
				dvm.Columns.Values.ToList().ForEach(x =>
					{
						if (!string.IsNullOrEmpty(x.ToString()))
							sum += Int32.Parse(x.ToString());
					});
				dvm.Sum = sum;
				this.dataViewModels.Add(dvm);
			}
			this.dataViewModels = this.dataViewModels.OrderByDescending(x => x.Sum).ToList();
			for (int i = 0; i < this.dataViewModels.Count; i++)
				this.dataViewModels[i].Placement = i + 1;

			foreach (var dvm in this.dataViewModels)
			{
				table.Rows.Add(dvm.ToObjectArray());
			}
			this.Data = table;
		}

		private List<Player> GetAllPlayersOfTournamentSeries(TournamentSeries tournamentSeries)
		{
			var ret = new List<Player>();
			foreach (var placement in from tournament in tournamentSeries.Tournaments
					where tournament.State == TournamentState.Closed from placement in tournament.Placements
					where !ret.Contains(placement.Player) select placement)
			{
				ret.Add(placement.Player);
			}
			return ret;
		}

		private void Start()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.PlayerSelection, new List<object>() { this.selectedSeries.Tournaments[this.actualTournamentIndex], this.selectedSeries } );
		}

		private bool CanStart()
		{
			return !string.IsNullOrEmpty(this.StartText);
		}

		private void Statistics()
		{
		}

		private void Print()
		{
		}

		private void HomeButton()
		{
			eventService.PublishDisplayChangedEvent(DisplayEnum.Home);
		}

		#endregion
	}
}
