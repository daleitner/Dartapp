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
