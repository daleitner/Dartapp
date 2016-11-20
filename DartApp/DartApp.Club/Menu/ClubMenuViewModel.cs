using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
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
		private ObservableCollection<DataViewModel> data = null;
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
		public ObservableCollection<DataViewModel> Data
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

				if (this.actualTournamentIndex >= 0)
					this.startText = "Turnier " + this.selectedSeries.Tournaments[this.actualTournamentIndex].Key + " starten";
			}
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
