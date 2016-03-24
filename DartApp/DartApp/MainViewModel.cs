using System;
using System.IO;
using System.Windows.Input;
using Base;
using DataBaseInitializer;
namespace DartApp
{
	public class MainViewModel : ViewModelBase
	{
		private ViewModelBase content;
		public MainViewModel()
		{
			this.content = new HomeViewModel();
			var setup = Directory.GetCurrentDirectory() + "\\database.xml";
			var testValueFile = Directory.GetCurrentDirectory() + "\\dbtestvalues.txt";
			var mappingPath = Directory.GetCurrentDirectory() + "\\mapping.xml";
			try
			{
				var dbc = DataBaseCreator.GetInstance(setup, mappingPath, testValueFile);
			}
			catch (Exception e)
			{
				
			}
		}

		public ViewModelBase Content
		{
			get
			{
				return this.content;
				
			}
			set
			{
				this.content = value;
				OnPropertyChanged("Content");
			}
		}
	}
}
