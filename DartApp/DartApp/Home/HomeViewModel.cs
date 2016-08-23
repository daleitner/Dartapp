using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Base;
using DartApp.Services;

namespace DartApp.Home
{
	public class HomeViewModel : ViewModelBase
	{
		private RelayCommand clubCommand;
		private RelayCommand trainingCommand;
		private RelayCommand vdsvCommand;
		private RelayCommand dataBaseCommand;
        public IEventService eventService;

		public HomeViewModel(IEventService eventService)
		{
            this.eventService = eventService;
		}

		public ICommand ClubCommand
		{
			get
			{
				return this.clubCommand ?? (this.clubCommand = new RelayCommand(
					param => OpenClub()
					));
			}
		}

		private void OpenClub()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		public ICommand TrainingCommand
		{
			get
			{
				return this.trainingCommand ?? (this.trainingCommand = new RelayCommand(
					param => OpenTraining(),
					param => CanOpenTraining()
					));
			}
		}

		private void OpenTraining()
		{

		}

		private bool CanOpenTraining()
		{
			return false;
		}

		public ICommand VdsvCommand
		{
			get
			{
				return this.vdsvCommand ?? (this.vdsvCommand = new RelayCommand(
					param => OpenVdsv(),
					param => CanOpenVdsv()
					));
			}
		}

		private void OpenVdsv()
		{

		}

		private bool CanOpenVdsv()
		{
			return false;
		}

		public ICommand DataBaseCommand
		{
			get
			{
				return this.dataBaseCommand ?? (this.dataBaseCommand = new RelayCommand(
					param => OpenDatabase()
					));
			}
		}

		private void OpenDatabase()
		{
            this.eventService.PublishDisplayChangedEvent(DisplayEnum.Database);
		}
	}
}
