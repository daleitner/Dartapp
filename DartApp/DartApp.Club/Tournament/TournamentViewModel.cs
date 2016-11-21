using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DartApp.Models;
using DartApp.Services;

namespace DartApp.Club.Tournament
{
	public class TournamentViewModel : ViewModelBase
	{
		#region members
		private Models.Tournament tournament;
		private RelayCommand cancelCommand;
		private RelayCommand saveCommand;
		private ObservableCollection<MatchViewModel> matches;
		private ObservableCollection<MatchViewModel> playableMatches; 
		private TournamentPlanViewModel tournamentPlan;
		private readonly IEventService eventService;
		private readonly IDartAppCommandService commandService;
		private readonly TournamentSeries series;
		private bool tournamentFinished = false;

		#endregion

		#region ctors
		public TournamentViewModel(Models.Tournament tournament, TournamentSeries series, IEventService eventService, IDartAppCommandService commandService)
		{
			this.series = series;
			this.commandService = commandService;
			this.tournament = tournament;
			this.tournamentPlan = new TournamentPlanViewModel(this.tournament, series.AdditionalColumns);
			this.matches = new ObservableCollection<MatchViewModel>();
			this.tournament.Matches.ForEach(x => 
				{
					var m = new MatchViewModel(x);
					m.MatchChanged +=m_MatchChanged;
					this.matches.Add(m);
				});
			KickFreilose();
			UpdatePlayableMatches();
			this.eventService = eventService;
		}
		#endregion

		#region properties
		public ICommand CancelCommand
		{
			get
			{
				if (this.cancelCommand == null)
				{
					this.cancelCommand = new RelayCommand(
						param => Cancel()
					);
				}
				return this.cancelCommand;
			}
		}
		public ICommand SaveCommand
		{
			get
			{
				if (this.saveCommand == null)
				{
					this.saveCommand = new RelayCommand(
						param => Save(),
						param => CanSave()
					);
				}
				return this.saveCommand;
			}
		}
		public ObservableCollection<MatchViewModel> Matches
		{
			get
			{
				return this.playableMatches;
			}
			set
			{
				this.playableMatches = value;
				OnPropertyChanged("Matches");
			}
		}
		public TournamentPlanViewModel TournamentPlan
		{
			get
			{
				return this.tournamentPlan;
			}
			set
			{
				this.tournamentPlan = value;
				OnPropertyChanged("TournamentPlan");
			}
		}
		#endregion

		#region private methods
		private void Cancel()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		private void Save()
		{
			this.tournament.State = TournamentState.Closed;
			this.tournamentPlan.Rankings.ToList().ForEach(x => this.tournament.Placements.Add(new Placement(x.Ranking, x.Player)));
			this.commandService.SaveTournament(this.tournament, this.series);
			this.commandService.SaveAdditionalColumnValues(this.TournamentPlan.AdditionalColumnValues.ToList());
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		private bool CanSave()
		{
			return this.tournamentFinished;
		}

		private void UpdatePlayableMatches()
		{
			var help = new ObservableCollection<MatchViewModel>();
			foreach (var match in this.matches)
			{
				if(!string.IsNullOrEmpty(match.Player1) && !string.IsNullOrEmpty(match.Player2)
					&& match.Player1Legs == "0" && match.Player2Legs == "0")
					help.Add(match);
			}
			this.Matches = help;
		}

		private void KickFreilose()
		{
			foreach (var match in this.matches)
			{
				if (match.Player2 == "FL")
				{
					match.Player1Legs = "1";
					match.OkCommand.Execute(null);
				}
				else if (match.Player1 == "FL")
				{
					match.Player2Legs = "1";
					match.OkCommand.Execute(null);
				}
			}
		}

		#endregion

		#region public methods
		void m_MatchChanged(Match match)
		{
			this.TournamentPlan.Results.First(x => x.GetMatchKey() == match.PositionKey).Refresh();
			var toRefresh = TournamentController.EndMatch(match, this.tournament);
			toRefresh.ForEach(i =>
			{
				this.TournamentPlan.Results.First(x => x.GetMatchKey() == i).Refresh();
				this.matches[i].Refresh();
			});
			var ranking = TournamentController.GetRanking(match, this.tournament);
			if (ranking > 0)
			{
				if (ranking == 1)
				{
					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = match.GetWinner();
					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking+1).Player = match.GetLoser();
					this.tournamentFinished = true;
				}
				else
				{

					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = match.GetLoser();
				}
				this.TournamentPlan.Refresh();
			}
			UpdatePlayableMatches();
		}
		#endregion
	}
}
