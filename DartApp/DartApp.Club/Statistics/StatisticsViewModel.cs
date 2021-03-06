using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Base;
using DartApp.CommandServices;
using DartApp.Models;
using DartApp.QueryService;
using DartApp.Services;

namespace DartApp.Club.Statistics
{
	public class StatisticsViewModel : ViewModelBase
	{
		#region members
		private RelayCommand homeCommand = null;
		private RelayCommand searchCommand = null;
		private RelayCommand updateCommand = null;
		private ObservableCollection<string> searchResult = null;
		private List<PlayerStatisticViewModel> statistics;
		private PlayerStatisticViewModel selectedStatistic;
		private string search = "";
		private readonly IDartAppQueryService queryService;
		private readonly IDartAppCommandService commandService;
		private readonly IEventService eventService;
		private readonly TournamentSeries series;
		#endregion

		#region ctors
		public StatisticsViewModel(IEventService evendService, IDartAppQueryService queryService, IDartAppCommandService commandService, TournamentSeries selectedSeries)
		{
			this.eventService = evendService;
			this.queryService = queryService;
			this.commandService = commandService;
			this.series = selectedSeries;
			this.statistics = new List<PlayerStatisticViewModel>();
			if(this.series.Tournaments.Count != 0)
				LoadData();
		}

		private void LoadData()
		{
			var stat = this.queryService.GetStatisticsByTournamentSeries(this.series);
			if (stat.Count == 0)
				stat = CreateStatistics();
			stat.ForEach(x => this.statistics.Add(new PlayerStatisticViewModel(x)));
			this.statistics = this.statistics.OrderByDescending(x => x.Points).ThenByDescending(x => x.SetDifference).ThenByDescending(x => x.LegDifference).ToList();
			for(int i = 1; i<= this.statistics.Count; i++)
			{
				this.statistics[i-1].Ranking = i;
			}
			this.statistics = this.statistics.OrderBy(x => x.Name).ToList();
		}

		private List<Statistic> CreateStatistics()
		{
			var ret = new List<Statistic>();
			if (this.series.Tournaments.Count(x => x.State == TournamentState.Closed) <= 0)
				return ret;

			var points = this.queryService.GetPlacementPoints();
			var allPlayers = GetAllPlayersOfTournamentSeries(this.series);
			foreach (var player in allPlayers)
			{
				var statistic = new Statistic(player, this.series);
				var played = 0;
				foreach (var tournament in this.series.Tournaments)
				{
					if (tournament.State != TournamentState.Closed)
						continue;

					//Points, first, second, third places
					bool hasPlayed = false;
					foreach (var placement in tournament.Placements)
					{
						if (placement.Player.Equals(player))
						{
							switch (placement.Position)
							{
								case 1:
									statistic.First++;
									break;
								case 2:
									statistic.Second++;
									break;
								case 3:
									statistic.Third++;
									break;
							}

							var tmp = placement.Position;
							while (points.FirstOrDefault(x => x.Position == tmp) == null)
								tmp--;
							statistic.Points += points.First(x => x.Position == tmp).Points;
							hasPlayed = true;
							played++;
							break;
						}
					}

					if(!hasPlayed)
						continue;

					//Legs, sets, FLs
					foreach (var match in tournament.Matches)
					{
						if (match.Player1.Equals(player))
						{
							if (!PlayerIsFreilos(match.Player2, allPlayers))
							{
								statistic.WonLegs += match.Player1Legs;
								statistic.LostLegs += match.Player2Legs;
								if (match.Player1Legs > match.Player2Legs)
									statistic.WonSets ++;
								else
									statistic.LostSets ++;
							}
							else
							{
								statistic.FLs++;
							}
						}
						else if (match.Player2.Equals(player))
						{
							if (!PlayerIsFreilos(match.Player1, allPlayers))
							{
								statistic.WonLegs += match.Player2Legs;
								statistic.LostLegs += match.Player1Legs;
								if (match.Player1Legs > match.Player2Legs)
									statistic.LostSets++;
								else
									statistic.WonSets++;
							}
							else
							{
								statistic.FLs++;
							}
						}
					}
				}
				statistic.Average = (int)Math.Round((double)statistic.Points/played,0);
				this.commandService.InsertStatistic(statistic);
				ret.Add(statistic);
			}
			return ret;
		}

		#endregion

		#region properties
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
						param => Searchf()
					);
				}
				return this.searchCommand;
			}
		}
		public ICommand UpdateCommand
		{
			get
			{
				if (this.updateCommand == null)
				{
					this.updateCommand = new RelayCommand(
						param => UpdateStatistics()
					);
				}
				return this.updateCommand;
			}
		}



		public List<PlayerStatisticViewModel> Statistics
		{
			get
			{
				return this.statistics;
			}
			set
			{
				this.statistics = value;
				OnPropertyChanged("Statistics");
			}
		}
		public PlayerStatisticViewModel SelectedStatistic
		{
			get
			{
				return this.selectedStatistic;
			}
			set
			{
				this.selectedStatistic = value;
				OnPropertyChanged("SelectedStatistic");
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
		#endregion

		#region private methods
		private void Home()
		{
			this.eventService.PublishDisplayChangedEvent(DisplayEnum.Club);
		}

		private void Searchf()
		{
		}

		private void UpdateStatistics()
		{
			var allPlayers = GetAllPlayersOfTournamentSeries(this.series);
			var placementPoints = this.queryService.GetPlacementPoints();
			var newStatistics = new List<Statistic>();
			foreach(var player in allPlayers)
			{
				var statistic = new Statistic(player, this.series);
				foreach(var tournament in this.series.Tournaments)
				{
					if (!tournament.GetAllPlayers().Contains(player))
						continue;

					var position = tournament.Placements.First(x => x.Player.Equals(player)).Position;
					//add first,second,third
					if (position == 1)
						statistic.First++;
					else if (position == 2)
						statistic.Second++;
					else if (position == 3)
						statistic.Third++;

					//add points
					var tmpPos = position;
					while (placementPoints.FirstOrDefault(x => x.Position == tmpPos) == null)
						tmpPos--;
					statistic.Points += placementPoints.First(x => x.Position == tmpPos).Points;

					//add Legs, Sets and FLs
					var matches = tournament.Matches.Where(x => x.Player1.Equals(player));
					foreach (var match in matches)
					{
						if (match.Player2.VorName == "FL")
							statistic.FLs++;
						else
						{
							statistic.WonLegs += match.Player1Legs;
							statistic.LostLegs += match.Player2Legs;
							if (match.Player1Legs < match.Player2Legs)
								statistic.LostSets++;
							else
								statistic.WonSets++;
						}
					}

					var matches2 = tournament.Matches.Where(x => x.Player2.Equals(player));
					foreach (var match in matches2)
					{
						if (match.Player1.VorName == "FL")
							statistic.FLs++;
						else
						{
							statistic.WonLegs += match.Player2Legs;
							statistic.LostLegs += match.Player1Legs;
							if (match.Player1Legs > match.Player2Legs)
								statistic.LostSets++;
							else
								statistic.WonSets++;
						}
					}
				}

				var oldStatistic = this.statistics.Select(x => x.statistic).FirstOrDefault(x => x.Player.Equals(player));
				if(oldStatistic == null)
					this.commandService.InsertStatistic(statistic);
				else if(oldStatistic.IsOutOfDate(statistic))
				{
					oldStatistic.Update(statistic);
					this.commandService.UpdateStatistic(oldStatistic); //oldStatistic wegen der Id
				}
				newStatistics.Add(statistic);
			}
			this.Statistics = new List<PlayerStatisticViewModel>(newStatistics.Select(x=> new PlayerStatisticViewModel(x)).OrderBy(x => x.Name));
		}

		private List<Player> GetAllPlayersOfTournamentSeries(TournamentSeries tournamentSeries)
		{
			var ret = new List<Player>();
			foreach (var tournament in tournamentSeries.Tournaments)
			{
				var players = tournament.GetAllPlayers();
				players.Where(p => !ret.Contains(p)).ToList().ForEach(p => ret.Add(p));
			}
			return ret;
		}

		private bool PlayerIsFreilos(Player player, List<Player> players)
		{
			return !Enumerable.Contains(players, player);
		}
		#endregion

		#region public methods
		#endregion
	}
}
