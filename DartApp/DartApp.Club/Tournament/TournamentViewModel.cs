using System;
using System.Collections.Generic;
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
		private RelayCommand undoCommand;
		private ObservableCollection<MatchViewModel> matches;
		private ObservableCollection<MatchViewModel> playableMatches;
		private List<Match> history;
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
			this.history = new List<Match>();
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
		public ICommand UndoCommand
		{
			get
			{
				if (this.undoCommand == null)
				{
					this.undoCommand = new RelayCommand(
						param => Undo(),
						param => CanUndo()
					);
				}
				return this.undoCommand;
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
			var columnValues = new List<AdditionalColumnValue>();
			foreach (var value in this.TournamentPlan.AdditionalColumnValues)
			{
				if (value.SelectedColumn != null && value.SelectedPlayer != null && !string.IsNullOrEmpty(value.Value))
					columnValues.Add(new AdditionalColumnValue { Player = value.SelectedPlayer, Column = value.SelectedColumn, Value = Int32.Parse(value.Value) });
			}
			this.commandService.SaveAdditionalColumnValues(columnValues);
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club, new List<object> { this.series});
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
					&& !match.IsEntered)
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

		private void Undo()
		{
			var match = this.history.Last();

			//Platzierungen entfernen
			var ranking = TournamentController.GetRanking(match, this.tournament);
			if (ranking > 0)
			{
				if (ranking == 1) //match is finale
				{
					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = null;
					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking + 1).Player = null;
					this.tournamentFinished = false;
					if (this.tournament.Matches.Count - 1 != match.PositionKey) //delete second match in finale
					{
						this.TournamentPlan.DeleteSecondResultInFinale();
						this.tournament.Matches.RemoveAt(this.tournament.Matches.Count-1);
						this.matches.RemoveAt(this.matches.Count-1);
					}
				}
				else //match is at loser side
				{

					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = null;
				}
				this.TournamentPlan.Refresh();
			}

			var toRefresh = TournamentController.UndoMatch(match, this.tournament);
			this.TournamentPlan.Results.First(x => x.GetMatchKey() == match.PositionKey).Refresh();
			toRefresh.ForEach(i =>
			{
				this.TournamentPlan.Results.First(x => x.GetMatchKey() == i).Refresh();
				this.matches[i].Refresh();
				this.matches[i].IsEntered = false;
			});

			this.matches.First(x => x.GetPositionKey() == match.PositionKey).IsEntered = false;

			UpdatePlayableMatches();

			this.history.Remove(match);
		}

		private bool CanUndo()
		{
			return this.history.Count > 0;
		}

		#endregion

		#region public methods
		//wird ausgeführt, wenn ein match eingetragen wird.
		void m_MatchChanged(Match match)
		{
			this.TournamentPlan.RefreshResult(match.PositionKey);
			var toRefresh = TournamentController.EndMatch(match, this.tournament);
			toRefresh.ForEach(i =>
			{
				this.TournamentPlan.RefreshResult(i);
				this.matches[i].Refresh();
			});

			//Platzierungen eintragen
			var ranking = TournamentController.GetRanking(match, this.tournament);
			if (ranking > 0)
			{
				if (ranking == 1) //match is finale
				{
					if (match.PositionKey % 2 == 1 && match.Player1Legs < match.Player2Legs)
					{
						var m = new Match(this.tournament.Matches.Count, match.Player1, match.Player2);
						this.tournament.Matches.Add(m);
						var mvm = new MatchViewModel(this.tournament.Matches.Last());
						mvm.MatchChanged += m_MatchChanged;
						this.matches.Add(mvm);
						this.matches.Last().Refresh();
						this.TournamentPlan.AddSecondResultForFinale();
					}
					else
					{
						this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = match.GetWinner();
						this.TournamentPlan.Rankings.First(x => x.Ranking == ranking+1).Player = match.GetLoser();
						this.tournamentFinished = true;
					}
				}
				else //match is at loser side
				{

					this.TournamentPlan.Rankings.First(x => x.Ranking == ranking).Player = match.GetLoser();
				}
				this.TournamentPlan.Refresh();
			}
			
			if (this.history != null)
			{
				this.history.Add(match);
			}
			toRefresh.ForEach(i =>
			{
				if (this.matches[i].Player1 == "FL" || this.matches[i].Player2 == "FL")
					this.matches[i].OkCommand.Execute(null);
			});
			UpdatePlayableMatches();
		}
		#endregion
	}
}
