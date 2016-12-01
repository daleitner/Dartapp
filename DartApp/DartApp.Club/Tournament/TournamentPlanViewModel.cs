using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DartApp.Models;

namespace DartApp.Club.Tournament
{
	public class TournamentPlanViewModel : ViewModelBase
	{
		private Models.Tournament tournament = null;
		private string title = null;
		private ObservableCollection<RankingViewModel> rankings = null;
		private List<ResultViewModel> results = null;
		private List<ResultViewModel> startResults = null;
		private List<ResultViewModel> winResults = null;
		private List<ResultViewModel> loseResults = null;
		private ResultViewModel finaleResult = null;
		private ObservableCollection<AdditionalColumnValueViewModel> additionalColumnValues;
		private RelayCommand addCommand = null;
		private RelayCommand removeCommand = null;
		public TournamentPlanViewModel(Models.Tournament tournament, List<AdditionalColumn> additionalColumns)
		{
			this.tournament = tournament;
			this.title = this.tournament.DisplayName;
			FillResults();
			this.rankings = new ObservableCollection<RankingViewModel>();
			var numberOfPlayers = this.tournament.Matches.Count / 2 + 1;
			for (int i = 1; i <= numberOfPlayers; i++)
			{
				this.rankings.Add(new RankingViewModel(i, null));
			}
			this.additionalColumnValues = new ObservableCollection<AdditionalColumnValueViewModel>() {new AdditionalColumnValueViewModel(tournament.GetAllPlayers(), additionalColumns)};
			this.Players = tournament.GetAllPlayers();
			this.Columns = additionalColumns;
		}

		private void FillResults()
		{
			this.results = new List<ResultViewModel>();
			this.tournament.Matches.ForEach(x => this.results.Add(new ResultViewModel(x)));
			this.startResults = new List<ResultViewModel>();
			this.winResults = new List<ResultViewModel>();
			this.loseResults = new List<ResultViewModel>();
			int amountStartResults = (this.results.Count / 2 + 1) / 2;
			for(int i = 0; i<this.results.Count; i++)
			{
				if (i < amountStartResults)
					this.startResults.Add(this.results[i]);
				else if (TournamentController.IsWinnerSide(i, amountStartResults * 2))
					this.winResults.Add(this.results[i]);
				else if (i == this.results.Count - 1)
					this.finaleResult = this.results[i];
				else
					this.loseResults.Add(this.results[i]);
			}
		}

		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				OnPropertyChanged("Title");
			}
		}

		public ObservableCollection<RankingViewModel> Rankings
		{
			get
			{
				return this.rankings;
			}
			set
			{
				this.rankings = value;
				OnPropertyChanged("Rankings");
			}
		}

		public List<ResultViewModel> Results
		{
			get
			{
				return this.results;
			}
			set
			{
				this.results = value;
				OnPropertyChanged("Results");
			}
		}

		public List<ResultViewModel> StartResults
		{
			get
			{
				return this.startResults;
			}
			set
			{
				this.startResults = value;
				OnPropertyChanged("StartResults");
			}
		}

		public List<ResultViewModel> WinResults
		{
			get
			{
				return this.winResults;
			}
			set
			{
				this.winResults = value;
				OnPropertyChanged("WinResults");
			}
		}

		public List<ResultViewModel> LoseResults
		{
			get
			{
				return this.loseResults;
			}
			set
			{
				this.loseResults = value;
				OnPropertyChanged("LoseResults");
			}
		}

		public ResultViewModel FinaleResult
		{
			get
			{
				return this.finaleResult;
			}
			set
			{
				this.finaleResult = value;
				OnPropertyChanged("FinaleResult");
			}
		}

		public ObservableCollection<AdditionalColumnValueViewModel> AdditionalColumnValues
		{
			get
			{
				return this.additionalColumnValues;
			}
			set
			{
				this.additionalColumnValues = value;
				OnPropertyChanged("AdditionalColumnValues");
			}
		}
		public ICommand AddCommand
		{
			get
			{
				if (this.addCommand == null)
				{
					this.addCommand = new RelayCommand(
						param => Add()
					);
				}
				return this.addCommand;
			}
		}
		public ICommand RemoveCommand
		{
			get
			{
				if (this.removeCommand == null)
				{
					this.removeCommand = new RelayCommand(
						param => Remove(),
						param => CanRemove()
					);
				}
				return this.removeCommand;
			}
		}

		public List<AdditionalColumn> Columns { get; set; }
		public List<Player> Players { get; set; }  

		internal void Refresh()
		{
			var help = new ObservableCollection<RankingViewModel>(this.Rankings);
			this.Rankings = new ObservableCollection<RankingViewModel>(help);
		}

		private void Add()
		{
			this.AdditionalColumnValues.Add(new AdditionalColumnValueViewModel(this.Players, this.Columns));
		}

		private void Remove()
		{
			this.AdditionalColumnValues.Remove(this.AdditionalColumnValues.Last());
		}

		private bool CanRemove()
		{
			return this.AdditionalColumnValues.Count > 1;
		}
	}
}
