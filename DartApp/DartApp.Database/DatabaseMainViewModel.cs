using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DartApp.Database.EditDialogs;
using DartApp.Database.ModelViews;
using DartApp.Models;
using DartApp.QueryService;
using DartApp.Services;

namespace DartApp.Database
{
	public class DatabaseMainViewModel : ViewModelBase
	{
		#region members
		private RelayCommand playerCommand;
		private RelayCommand tournamentSerialCommand;
		private RelayCommand newCommand;
		private RelayCommand editCommand;
		private RelayCommand deleteCommand;
		private RelayCommand homeCommand;
		private RelayCommand searchCommand;
		private ModelBase selectedItem;
		private ObservableCollection<ModelBase> searchResult;
		private string header = "";
		private string search = "";
		private ViewModelBase specificView;
		private ModelEnum modelEnum = ModelEnum.Default;
        private readonly IEventService eventService;
		private readonly IDartAppQueryService queryService;
		private readonly IDartAppCommandService commandService;
		private AddPlayerWindow addPlayerWindow;
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
				OnSearch();
				this.SelectedItem = newTournamentSeries;
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
			return true;
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
			}
		}

		private bool CanEdit()
		{
			return this.SelectedItem != null;
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

		private void Delete()
		{
			MessageBoxResult ret = MessageBox.Show("Willst du " + this.SelectedItem + " wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (ret == MessageBoxResult.Yes)
			{
				this.commandService.DeletePlayer((Player)this.SelectedItem);
				this.SearchResult.Remove(this.SelectedItem);
				this.SelectedItem = this.SearchResult.FirstOrDefault();
			}
		}

		private bool CanDelete()
		{
			return this.SelectedItem != null && this.modelEnum != ModelEnum.TournamentSeries;
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

		private bool CanSelectTournamentSeries()
		{
			return this.modelEnum != ModelEnum.TournamentSeries;
		}

		#endregion

		#region public methods
		#endregion
	}
}
