using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Base;
using DartApp.Models;
using DartApp.Services;
using DartApp.Database.ModelViews;
using DartApp.QueryService;
using DartApp.Database.EditDialogs;
using DartApp.CommandServices;

namespace DartApp.Database
{
	public class DatabaseMainViewModel : ViewModelBase
	{
		#region members
		private RelayCommand playerCommand = null;
		private RelayCommand holidayCommand = null;
		private RelayCommand newCommand = null;
		private RelayCommand editCommand = null;
		private RelayCommand deleteCommand = null;
		private RelayCommand homeCommand = null;
		private RelayCommand searchCommand = null;
		private ModelBase selectedItem = null;
		private ObservableCollection<ModelBase> searchResult = null;
		private string header = "";
		private string search = "";
		private ViewModelBase specificView = null;
		private ModelEnum modelEnum = ModelEnum.Default;
        private IEventService eventService;
		private IDartAppQueryService queryService;
		private IDartAppCommandService commandService;
		private AddPlayerWindow addPlayerWindow;
		#endregion

		#region ctors
		public DatabaseMainViewModel(IEventService eventService, IDartAppQueryService queryService, IDartAppCommandService commandService)
		{
			this.queryService = queryService;
			this.commandService = commandService;
            this.eventService = eventService;
		}
		#endregion

		#region properties
		public ICommand PlayerCommand
		{
			get
			{
				if (this.playerCommand == null)
				{
					this.playerCommand = new RelayCommand(
						param => RefreshView(ModelEnum.Player),
						param => CanSelectPlayer()
					);
				}
				return this.playerCommand;
			}
		}

		public ICommand HolidayCommand
		{
			get
			{
				if (this.holidayCommand == null)
				{
					this.holidayCommand = new RelayCommand(
						param => RefreshView(ModelEnum.Holiday),
						param => CanSelectHoliday()
					);
				}
				return this.holidayCommand;
			}
		}

		public ICommand NewCommand
		{
			get
			{
				if (this.newCommand == null)
				{
					this.newCommand = new RelayCommand(
						param => New()
					);
				}
				return this.newCommand;
			}
		}
		public ICommand EditCommand
		{
			get
			{
				if (this.editCommand == null)
				{
					this.editCommand = new RelayCommand(
						param => Edit()
					);
				}
				return this.editCommand;
			}
		}
		public ICommand DeleteCommand
		{
			get
			{
				if (this.deleteCommand == null)
				{
					this.deleteCommand = new RelayCommand(
						param => Delete()
					);
				}
				return this.deleteCommand;
			}
		}
		public ICommand HomeCommand
		{
			get
			{
				if (this.homeCommand == null)
				{
					this.homeCommand = new RelayCommand(
						param => Home()
					);
				}
				return this.homeCommand;
			}
		}
		public ICommand SearchCommand
		{
			get
			{
				if (this.searchCommand == null)
				{
					this.searchCommand = new RelayCommand(
						param => OnSearch()
					);
				}
				return this.searchCommand;
			}
		}
		public ModelBase SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				this.selectedItem = value;
				UpdateSpecificModel();
				OnPropertyChanged("SelectedItem");
			}
		}
		public ObservableCollection<ModelBase> SearchResult
		{
			get
			{
				return this.searchResult;
			}
			set
			{
				this.searchResult = value;
				OnPropertyChanged("SearchResult");
			}
		}
		public string Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
				OnPropertyChanged("Header");
			}
		}
		public string Search
		{
			get
			{
				return this.search;
			}
			set
			{
				this.search = value;
				OnPropertyChanged("Search");
			}
		}
		public ViewModelBase SpecificView
		{
			get
			{
				return this.specificView;
			}
			set
			{
				this.specificView = value;
				OnPropertyChanged("SpecificView");
			}
		}
		#endregion

		#region private methods
		private void New()
		{
			switch (this.modelEnum)
			{
				case ModelEnum.Player:
					var pvm = new AddPlayerViewModel();
					pvm.ButtonClicked += pvm_ButtonClicked;
					this.addPlayerWindow = new AddPlayerWindow();
					this.addPlayerWindow.DataContext = pvm;
					this.addPlayerWindow.ShowDialog();
					break;
			}
		}

		void pvm_ButtonClicked(Player newPlayer)
		{
			if (newPlayer != null)
			{
				this.commandService.InsertPlayer(newPlayer);
				OnSearch();
			}
			this.addPlayerWindow.Close();
		}

		private void Edit()
		{
		}

		private void Delete()
		{
		}

		private void Home()
		{
            this.eventService.PublishDisplayChangedEvent(DisplayEnum.Home);
		}

		private void OnSearch()
		{
			this.SearchResult = new ObservableCollection<ModelBase>(this.queryService.GetSearchResult(this.Search, this.modelEnum));
		}

		private void RefreshView(ModelEnum modelEnum)
		{
			this.modelEnum = modelEnum;
			OnSearch();
			UpdateSpecificModel();
		}

		private void UpdateSpecificModel()
		{
			if (this.SelectedItem != null)
			{
				switch (this.modelEnum)
				{
					case ModelEnum.Player:
					case ModelEnum.Holiday:
						this.SpecificView = new PlayerViewModel((Player)this.SelectedItem);
						break;
				}
			}
		}

		private bool CanSelectPlayer()
		{
			return this.modelEnum != ModelEnum.Player;
		}

		private bool CanSelectHoliday()
		{
			return this.modelEnum != ModelEnum.Holiday;
		}

		private void UpdateSpecificView(ModelEnum modelEnum)
		{
			this.modelEnum = modelEnum;
			switch(modelEnum)
			{
				case ModelEnum.Player: 
					this.SpecificView = new PlayerViewModel(new Player("Daniel", "Leitner", new DateTime(1991, 11, 29), "test.jpg")); 
					break;
			}
		}

		private bool CanSelectPlayer()
		{
			return this.modelEnum != ModelEnum.Player;
		}

		#endregion

		#region public methods
		#endregion
	}
}
