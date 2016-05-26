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
		private RelayCommand holidayCommand;
		private RelayCommand trainingCommand;
		private RelayCommand vdsvCommand;
		private RelayCommand dataBaseCommand;
        public IEventService eventService;

		public HomeViewModel(IEventService eventService)
		{
            this.eventService = eventService;
		}

		public ICommand HolidayCommand
		{
			get
			{
				return this.holidayCommand ?? (this.holidayCommand = new RelayCommand(
					param => OpenHoliday()
					));
			}
		}

		private void OpenHoliday()
		{

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
