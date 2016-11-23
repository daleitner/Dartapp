using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using DartApp.Models;

namespace DartApp.Club.Tournament
{
	public class AdditionalColumnValueViewModel : ViewModelBase
	{
		#region members
		private AdditionalColumn selectedColumn = null;
		private Player selectedPlayer = null;
		private string value = "";
		#endregion

		#region ctors
		public AdditionalColumnValueViewModel(List<Player> allPlayers, List<AdditionalColumn> allColumns)
		{
			this.Players = allPlayers;
			this.Columns = allColumns;
		}
		#endregion

		#region properties
		public AdditionalColumn SelectedColumn
		{
			get
			{
				return this.selectedColumn;
			}
			set
			{
				this.selectedColumn = value;
				OnPropertyChanged("SelectedColumn");
			}
		}

		public Player SelectedPlayer
		{
			get
			{
				return this.selectedPlayer;
			}
			set
			{
				this.selectedPlayer = value;
				OnPropertyChanged("SelectedPlayer");
			}
		}
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				OnPropertyChanged("Value");
			}
		}

		public List<Player> Players { get; set; }
		public List<AdditionalColumn> Columns { get; set; }  
		#endregion
	}
}
