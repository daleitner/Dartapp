using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Base;
using DartApp.Services;

namespace DartApp.Club.Tournament
{
	public class TournamentViewModel : ViewModelBase
	{
		#region members
		private Models.Tournament tournament = null;
		private RelayCommand cancelCommand = null;
		private RelayCommand saveCommand = null;
		private ObservableCollection<MatchViewModel> matches = null;
		private TournamentPlanViewModel tournamentPlan = null;
		private IEventService eventService;
		#endregion

		#region ctors
		public TournamentViewModel(Models.Tournament tournament, IEventService eventService)
		{
			this.tournament = tournament;
			this.tournamentPlan = new TournamentPlanViewModel(this.tournament);
			this.matches = new ObservableCollection<MatchViewModel>();
			this.tournament.Matches.ForEach(x => 
				{
					var m = new MatchViewModel(x);
					m.MatchChanged +=m_MatchChanged;
					this.matches.Add(m);
				});
			this.eventService = eventService;
		}

		void m_MatchChanged(Models.Match match)
		{
			this.TournamentPlan.Results.Where(x => x.GetMatchKey() == match.PositionKey).First().Refresh();
			var toRefresh = TournamentController.EndMatch(match, this.tournament);
			toRefresh.ForEach(i =>
				{ 
					this.TournamentPlan.Results.Where(x => x.GetMatchKey() == i).First().Refresh();
					this.Matches[i].Refresh();
				});

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
						param => Save()
					);
				}
				return this.saveCommand;
			}
		}
		public ObservableCollection<MatchViewModel> Matches
		{
			get
			{
				return this.matches;
			}
			set
			{
				this.matches = value;
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
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		#endregion

		#region public methods
		#endregion
	}
}
