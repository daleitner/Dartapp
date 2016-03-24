using System;
using System.IO;
using System.Windows.Input;
using Base;
using DataBaseInitializer;
namespace DartApp
{
	public class MainViewModel : ViewModelBase
	{
		private string state;
		private RelayCommand holidayCommand;
		private RelayCommand trainingCommand;
		private RelayCommand vdsvCommand;
		public MainViewModel()
		{
			string setup = Directory.GetCurrentDirectory() + "\\database.xml";
			string testValueFile = Directory.GetCurrentDirectory() + "\\dbtestvalues.txt";
			string mappingPath = Directory.GetCurrentDirectory() + "\\mapping.xml";
			try
			{
				var dbc = DataBaseCreator.GetInstance(setup, mappingPath, testValueFile);
				this.State = "database ready";
			}
			catch (Exception e)
			{
				this.State = e.Message;
			}
			
			
		}

		public string State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				OnPropertyChanged("State");
			}
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
	}
}
