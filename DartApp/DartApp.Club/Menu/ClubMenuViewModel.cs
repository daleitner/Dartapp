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
		public ClubMenuViewModel(TournamentSeries selectedSeries, IDartAppQueryService queryService, IEventService eventService)
		{
			this.eventService = eventService;
			this.queryService = queryService;
			LoadData(selectedSeries);
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
				UpdateData();
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

		private void LoadData(TournamentSeries selectedSeries)
		{
			this.points = this.queryService.GetPlacementPoints();
			this.series = this.queryService.GetTournamentSeries();
			if(selectedSeries != null)
				this.selectedSeries = this.series.FirstOrDefault(x => x.GetId() == selectedSeries.GetId());
			if (this.selectedSeries == null)
				this.selectedSeries = this.series.FirstOrDefault();
			UpdateData();
		}

		private void UpdateData()
		{
			this.Header = this.selectedSeries.Name;
			if (this.selectedSeries.Tournaments == null || this.selectedSeries.Tournaments.Count == 0)
				this.selectedSeries = this.queryService.GetFullTournamentSeries(this.selectedSeries);
			var table = new DataTable();
			table.Columns.Add("Platz");
			table.Columns.Add("Name");
			this.selectedSeries.AdditionalColumns.ForEach(x => table.Columns.Add(x.Name));
			this.selectedSeries.Tournaments.ForEach(x => table.Columns.Add(x.Key.ToString()));
			table.Columns.Add("Gesamt");
			var allPlayers = GetAllPlayersOfTournamentSeries(this.selectedSeries);
			this.dataViewModels = new List<DataViewModel>();
			foreach (var player in allPlayers)
			{
				var dvm = new DataViewModel {Name = player.VorName + " " + player.NachName};
				foreach (var column in this.selectedSeries.AdditionalColumns)
				{
					bool found = false;
					foreach (var value in column.Values)
					{
						if (value.Player.Equals(player))
						{
							dvm.Columns.Add(column.Name, value.Value);
							found = true;
						}
					}
					if(!found)
						dvm.Columns.Add(column.Name, "");
				}
				var sum = 0;
				var pointsList = new List<int>();
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
								//sum += p;
								pointsList.Add(p);
								found = true;
								break;
							}
						}
						if (!found)
						{
							dvm.Columns.Add(tournament.Key.ToString(), "");
							pointsList.Add(0);
						}
					}
					else
					{
						dvm.Columns.Add(tournament.Key.ToString(), "");
						pointsList.Add(0);
					}
				}
				pointsList.Sort();
				for (int i = pointsList.Count-1; i >= pointsList.Count - this.selectedSeries.RelevantTournaments; i--)
					sum += pointsList[i];
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

			if (this.selectedSeries != null)
			{
				for (int i = 0; i < this.selectedSeries.Tournaments.Count; i++)
				{
					if (this.selectedSeries.Tournaments[i].State != TournamentState.Closed)
					{
						this.actualTournamentIndex = i;
						break;
					}
				}

				if (this.actualTournamentIndex >= 0)
					this.StartText = "Turnier " + this.selectedSeries.Tournaments[this.actualTournamentIndex].Key + " starten";
			}
		}

		private List<Player> GetAllPlayersOfTournamentSeries(TournamentSeries tournamentSeries)
		{
			var ret = new List<Player>();
			foreach(var tournament in tournamentSeries.Tournaments)
			{
				var players = tournament.GetAllPlayers();
				players.Where(p => !ret.Contains(p)).ToList().ForEach(p => ret.Add(p));
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
