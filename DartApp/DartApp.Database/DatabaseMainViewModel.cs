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
using System.Windows;

namespace DartApp.Database
{
	public class DatabaseMainViewModel : ViewModelBase
	{
		#region members
		private RelayCommand playerCommand = null;
		private RelayCommand holidayCommand = null;
		private RelayCommand tournamentSerialCommand = null;
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
		private EditHolidayWindow editHolidayWindow;
		private AddTournamentSeriesWindow addTournamentSeriesWindow;
		#endregion

		#region ctors
		public DatabaseMainViewModel(IEventService eventService, IDartAppQueryService queryService, IDartAppCommandService commandService)
		{
			this.queryService = queryService;
			this.commandService = commandService;
            this.eventService = eventService;
			this.modelEnum = ModelEnum.Player;
			RefreshView(this.modelEnum);
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

		public ICommand TournamentSerialCommand
		{
			get
			{
				if (this.tournamentSerialCommand == null)
				{
					this.tournamentSerialCommand = new RelayCommand(
						param => RefreshView(ModelEnum.TournamentSeries),
						param => CanSelectTournamentSeries()
					);
				}
				return this.tournamentSerialCommand;
			}
		}

		public ICommand NewCommand
		{
			get
			{
				if (this.newCommand == null)
				{
					this.newCommand = new RelayCommand(
						param => New(),
						param => CanNew()
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
						param => Edit(),
						param => CanEdit()
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
						param => Delete(),
						param => CanDelete()
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
					pvm.ButtonClicked += E_SaveNewPlayer;
					this.addPlayerWindow = new AddPlayerWindow();
					this.addPlayerWindow.DataContext = pvm;
					this.addPlayerWindow.ShowDialog();
					break;
				case ModelEnum.TournamentSeries:
					var tvm = new AddTournamentSeriesViewModel(this.queryService);
					tvm.ButtonClicked += E_SaveNewTournamentSeries;
					this.addTournamentSeriesWindow = new AddTournamentSeriesWindow {DataContext = tvm};
					this.addTournamentSeriesWindow.ShowDialog();
					break;
			}
		}

		private void E_SaveNewTournamentSeries(TournamentSeries newTournamentSeries)
		{
			if (newTournamentSeries != null)
			{
				this.commandService.InsertTournamentSeries(newTournamentSeries);
				var players = this.queryService.GetAllHolidayPlayers();
				foreach (var player in players)
				{
					var stat = new Statistic(player, newTournamentSeries);
					this.commandService.InsertStatistic(stat);
				}
			}
			this.addTournamentSeriesWindow.Close();
		}

		void E_SaveNewPlayer(Player newPlayer)
		{
			if (newPlayer != null)
			{
				this.commandService.InsertPlayer(newPlayer);
				OnSearch();
				this.SelectedItem = newPlayer;
			}
			this.addPlayerWindow.Close();
		}

		private bool CanNew()
		{
			return this.modelEnum != ModelEnum.Holiday;
		}

		private void Edit()
		{
			switch (this.modelEnum)
			{
				case ModelEnum.Player:
					var pvm = new AddPlayerViewModel((Player)this.SelectedItem);
					pvm.ButtonClicked += E_UpdatePlayer;
					this.addPlayerWindow = new AddPlayerWindow();
					this.addPlayerWindow.DataContext = pvm;
					this.addPlayerWindow.ShowDialog();
					break;
				case ModelEnum.Holiday:
					var evm = new EditHolidayViewModel(this.queryService);
					evm.ButtonClicked += E_UpdateHolidayList;
					this.editHolidayWindow = new EditHolidayWindow();
					this.editHolidayWindow.DataContext = evm;
					this.editHolidayWindow.ShowDialog();
					break;
			}
		}

		private bool CanEdit()
		{
			return this.SelectedItem != null || this.modelEnum == ModelEnum.Holiday;
		}

		void E_UpdatePlayer(Player newPlayer)
		{
			if (newPlayer != null)
			{
				this.commandService.UpdatePlayer(newPlayer);
				OnSearch();
			}
			this.addPlayerWindow.Close();
			this.SelectedItem = newPlayer;
		}

		void E_UpdateHolidayList(List<Player> newHolidayPlayers)
		{
			this.editHolidayWindow.Close();
			if (newHolidayPlayers == null)
				return;

			var oldHolidayPlayers = this.queryService.GetAllHolidayPlayers();

			List<Player> toAdd = new List<Player>();
			foreach (var np in newHolidayPlayers)
			{
				var found = false;
				foreach (var op in oldHolidayPlayers)
				{
					if (found) break;
					if (op.GetId() == np.GetId())
						found = true;
				}
				if (!found)
					toAdd.Add(np);
			}

			List<Player> toRemove = new List<Player>();
			foreach (var op in oldHolidayPlayers)
			{
				var found = false;
				foreach (var np in newHolidayPlayers)
				{
					if (found) break;
					if (op.GetId() == np.GetId())
						found = true;
				}
				if (!found)
					toRemove.Add(op);
			}

			toAdd.ForEach(x => this.commandService.AddToHoliday(x));
			toRemove.ForEach(x => this.commandService.RemoveFromHoliday(x));
			RefreshView(ModelEnum.Holiday);
		}

		private void Delete()
		{
			MessageBoxResult ret = MessageBox.Show("Willst du " + this.SelectedItem.ToString() + " wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (ret == MessageBoxResult.Yes)
			{
				this.commandService.DeletePlayer((Player)this.SelectedItem);
				this.SearchResult.Remove(this.SelectedItem);
				this.SelectedItem = this.SearchResult.FirstOrDefault();
			}
		}

		private bool CanDelete()
		{
			return this.SelectedItem != null && this.modelEnum != ModelEnum.Holiday && this.modelEnum != ModelEnum.TournamentSeries;
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
					case ModelEnum.TournamentSeries:
						this.SpecificView = new TournamentSeriesViewModel((TournamentSeries) this.SelectedItem);
						break;
				}
			}
			else
			{
				this.SpecificView = null;
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

		private bool CanSelectTournamentSeries()
		{
			return this.modelEnum != ModelEnum.TournamentSeries;
		}

		#endregion

		#region public methods
		#endregion
	}
}
