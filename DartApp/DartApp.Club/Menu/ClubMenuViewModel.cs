using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Base;
using DartApp.Models;
using DartApp.QueryService;
using DartApp.Services;

namespace DartApp.Club.Menu
{
	public class ClubMenuViewModel : ViewModelBase
	{
		#region members
		private RelayCommand startCommand;
		private RelayCommand statisticsCommand;
		private RelayCommand printCommand;
		private RelayCommand homeButtonCommand;
		private DataTable data;
		private List<DataViewModel> dataViewModels; 
		private List<PlacementPoint> points;
		private List<TournamentSeries> series;
		private TournamentSeries selectedSeries;
		private string startText = "";
		private int actualTournamentIndex = -1;
		private bool showPoints = true;
		private bool showAdditionalValues = true;
		private bool showLegRatio = true;
		private bool showSetRatio = true;
		private readonly IEventService eventService;
		private readonly IDartAppQueryService queryService;
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
						param => Print(),
						param => CanPrint()
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

		public bool ShowPoints
		{
			get { return this.showPoints; }
			set
			{
				this.showPoints = value;
				OnPropertyChanged("ShowPoints");
				UpdateData();
			}
		}

		public bool ShowAdditionalValues
		{
			get { return this.showAdditionalValues; }
			set
			{
				this.showAdditionalValues = value;
				OnPropertyChanged("ShowAdditionalValues");
				UpdateData();
			}
		}

		public bool ShowLegRatio
		{
			get { return this.showLegRatio; }
			set
			{
				this.showLegRatio = value;
				OnPropertyChanged("ShowLegRatio");
				UpdateData();
			}
		}

		public bool ShowSetRatio
		{
			get { return this.showSetRatio; }
			set
			{
				this.showSetRatio = value;
				OnPropertyChanged("ShowSetRatio");
				UpdateData();
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
			if (this.selectedSeries == null)
				return;
			//Get full tournamentseries if nessecary
			if (this.selectedSeries.Tournaments == null || this.selectedSeries.Tournaments.Count == 0)
				this.selectedSeries = this.queryService.GetFullTournamentSeries(this.selectedSeries);

			//define the columns
			var table = new DataTable();
			table.Columns.Add("Platz");
			table.Columns.Add("Name");
			if(this.ShowAdditionalValues)
				this.selectedSeries.AdditionalColumns.ForEach(x => table.Columns.Add(x.Name));
			if (this.ShowSetRatio)
			{
				table.Columns.Add("Sets+");
				table.Columns.Add("Sets-");
				table.Columns.Add("Sets Diff");
			}
			if (this.ShowLegRatio)
			{
				table.Columns.Add("Legs+");
				table.Columns.Add("Legs-");
				table.Columns.Add("Legs Diff");
			}
			if(this.showPoints)
				this.selectedSeries.Tournaments.ForEach(x => table.Columns.Add(x.Key.ToString()));
			table.Columns.Add("Gesamt");

			//fill rows
			var allPlayers = GetAllPlayersOfTournamentSeries(this.selectedSeries);
			this.dataViewModels = new List<DataViewModel>();
			var fullDataViewModels = new List<DataViewModel>();
			foreach (var player in allPlayers)
			{
				var dvm = new DataViewModel {Name = player.VorName + " " + player.NachName};

				//additional values
				foreach (var column in this.selectedSeries.AdditionalColumns)
				{
					bool found = false;
					foreach (var value in column.Values)
					{
						if (value.Player.Equals(player))
						{
							dvm.AdditionalColumns.Add(column.Name, value.Value);
							found = true;
						}
					}
					if (!found)
						dvm.AdditionalColumns.Add(column.Name, "");
				}
				var sum = 0;
				var legsplus = 0;
				var legsminus = 0;
				var setsplus = 0;
				var setsminus = 0;
				var pointsList = new List<int>();
				foreach (var tournament in this.selectedSeries.Tournaments)
				{
					if (tournament.State == TournamentState.Closed)
					{
						//Placement Points
						bool found = false;
						foreach (var placement in tournament.Placements)
						{
							if (placement.Player.Equals(player))
							{
								var tmp = placement.Position;
								while (this.Points.FirstOrDefault(x => x.Position == tmp) == null)
									tmp--;
								var p = this.Points.First(x => x.Position == tmp).Points;
								dvm.Points.Add(tournament.Key.ToString(), p);
								pointsList.Add(p);
								found = true;
								break;
							}
						}
						if (!found)
						{
							dvm.Points.Add(tournament.Key.ToString(), "");
							pointsList.Add(0);
						}

						//Leg Ratio, Set Ratio
						foreach (var match in tournament.Matches)
						{
							if (match.Player1.Equals(player))
							{
								if (!PlayerIsFreilos(match.Player2, allPlayers))
								{
									legsplus += match.Player1Legs;
									legsminus += match.Player2Legs;
									if (match.Player1Legs > match.Player2Legs)
										setsplus ++;
									else
										setsminus++;
								}
							}
							else if (match.Player2.Equals(player))
							{
								if (!PlayerIsFreilos(match.Player1, allPlayers))
								{
									legsplus += match.Player2Legs;
									legsminus += match.Player1Legs;
									if (match.Player1Legs > match.Player2Legs)
										setsminus++;
									else
										setsplus++;
								}
							}
						}
					}
					else
					{
						dvm.Points.Add(tournament.Key.ToString(), "");
						pointsList.Add(0);
					}
				}

				dvm.LegRatios.Add("Legs+", legsplus);
				dvm.LegRatios.Add("Legs-", legsminus);
				dvm.LegRatios.Add("Leg Diff", legsplus-legsminus);

				dvm.SetRatios.Add("Sets+", setsplus);
				dvm.SetRatios.Add("Sets-", setsminus);
				dvm.SetRatios.Add("Set Diff", setsplus - setsminus);

				var pointsOrderIndexList = GetOrderIndicess(pointsList);
				pointsOrderIndexList = pointsOrderIndexList.GetRange(0, pointsOrderIndexList.Count - this.selectedSeries.RelevantTournaments);
				//pointsList.Sort();
				for (int i = 0; i < pointsList.Count; i++)
				{
					if (pointsOrderIndexList.Contains(i))
						continue;
					sum += pointsList[i];
				}
				dvm.Sum = sum;
				fullDataViewModels.Add(dvm);
			}
			fullDataViewModels = fullDataViewModels.OrderByDescending(x => x.Sum).ThenByDescending(x => x.SetRatios["Set Diff"]).ThenByDescending(x => x.LegRatios["Leg Diff"]).ToList();
			for (int i = 0; i < fullDataViewModels.Count; i++)
				fullDataViewModels[i].Placement = i + 1;

			foreach (var dataViewModel in fullDataViewModels)
			{
				var dvm = new DataViewModel
				{
					Sum = dataViewModel.Sum,
					Name = dataViewModel.Name,
					Placement = dataViewModel.Placement
				};

				if (this.ShowAdditionalValues)
					dvm.AdditionalColumns = dataViewModel.AdditionalColumns;
				if (this.showSetRatio)
					dvm.SetRatios = dataViewModel.SetRatios;
				if (this.ShowLegRatio)
					dvm.LegRatios = dataViewModel.LegRatios;
				if (this.ShowPoints)
					dvm.Points = dataViewModel.Points;
				this.dataViewModels.Add(dvm);
			}

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

		private bool PlayerIsFreilos(Player player, List<Player> players)
		{
			return !Enumerable.Contains(players, player);
		}

		private List<int> GetOrderIndicess(List<int> values)
		{
			var ret = new List<int>();
			for (int i = 0; i < values.Count; i++)
			{
				int min = 100;
				int index = -1;
				for (int j = 0; j < values.Count; j++)
				{
					if (ret.Contains(j))
						continue;
					if (values[j] < min)
					{
						min = values[j];
						index = j;
					}
				}
				ret.Add(index);
			}
			return ret;
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
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.PlayerSelection, new List<object> { this.selectedSeries.Tournaments[this.actualTournamentIndex], this.selectedSeries } );
		}

		private bool CanStart()
		{
			return !string.IsNullOrEmpty(this.StartText);
		}

		private bool CanPrint()
		{
			return this.SelectedSeries != null;
		}

		private void Statistics()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Statistic, new List<object>() {this.selectedSeries});
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
