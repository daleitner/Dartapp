using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Tournament
{
	public class TournamentPlanViewModel : ViewModelBase
	{
		private Models.Tournament tournament = null;
		private string title = null;
		private ObservableCollection<RankingViewModel> rankings = null;
		private List<ResultViewModel> results = null;
		public TournamentPlanViewModel(Models.Tournament tournament)
		{
			this.tournament = tournament;
			this.title = this.tournament.DisplayName;
			this.results = new List<ResultViewModel>();
			this.tournament.Matches.ForEach(x => this.results.Add(new ResultViewModel(x)));
			this.rankings = new ObservableCollection<RankingViewModel>();
			var numberOfPlayers = this.tournament.Matches.Count / 2 + 1;
			for (int i = 1; i <= numberOfPlayers; i++)
			{
				this.rankings.Add(new RankingViewModel(i, null));
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

		internal void Refresh()
		{
			var help = new ObservableCollection<RankingViewModel>(this.Rankings);
			this.Rankings = new ObservableCollection<RankingViewModel>(help);
		}
	}
}
