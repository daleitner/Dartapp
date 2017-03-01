using System.Collections.ObjectModel;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DartApp.Models;
using DartApp.QueryService;

namespace DartApp.Club.Statistics
{
	public class StatisticsViewModel : ViewModelBase
	{
		#region members
		private RelayCommand homeCommand = null;
		private RelayCommand searchCommand = null;
		private ObservableCollection<string> searchResult = null;
		private string selectedItem = "";
		private string search = "";
		private readonly IDartAppQueryService queryService;
		private readonly IDartAppCommandService commandService;
		private readonly TournamentSeries series;
		#endregion

		#region ctors
		public StatisticsViewModel(IDartAppQueryService queryService, IDartAppCommandService commandService, TournamentSeries selectedSeries)
		{
			this.queryService = queryService;
			this.commandService = commandService;
			this.series = selectedSeries;
			if(this.series.Tournaments.Count != 0)
				LoadData();
		}

		private void LoadData()
		{
			var statistics = this.queryService.GetStatisticsByTournamentSeries(this.series);
			if (statistics.Count == 0)
				CreateStatistics();
		}

		private void CreateStatistics()
		{
			
		}

		#endregion

		#region properties
		public ICommand HomeCommand
		{
			get
			{
				if (this.homeCommand == null)
				{
					this.homeCommand = new RelayCommand(
						param => Home()
					);
				}
				return this.homeCommand;
			}
		}
		public ICommand SearchCommand
		{
			get
			{
				if (this.searchCommand == null)
				{
					this.searchCommand = new RelayCommand(
						param => Searchf()
					);
				}
				return this.searchCommand;
			}
		}
		public ObservableCollection<string> SearchResult
		{
			get
			{
				return this.searchResult;
			}
			set
			{
				this.searchResult = value;
				OnPropertyChanged("SearchResult");
			}
		}
		public string SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				this.selectedItem = value;
				OnPropertyChanged("SelectedItem");
			}
		}
		public string Search
		{
			get
			{
				return this.search;
			}
			set
			{
				this.search = value;
				OnPropertyChanged("Search");
			}
		}
		#endregion

		#region private methods
		private void Home()
		{
		}

		private void Searchf()
		{
		}

		#endregion

		#region public methods
		#endregion
	}
}
