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
		private List<TournamentSeries> saisons = null;
		private TournamentSeries selectedSaison = null;
		private string startText = "";
		private string header = "";
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
						param => Start()
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
		public List<TournamentSeries> Saisons
		{
			get
			{
				return this.saisons;
			}
			set
			{
				this.saisons = value;
				OnPropertyChanged("Saisons");
			}
		}
		public TournamentSeries SelectedSaison
		{
			get
			{
				return this.selectedSaison;
			}
			set
			{
				this.selectedSaison = value;
				OnPropertyChanged("SelectedSaison");
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
			this.saisons = this.queryService.GetTournamentSeries();
			this.selectedSaison = this.saisons.FirstOrDefault();
		}

		private void Start()
		{
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

		#region public methods
		#endregion
	}
}
