using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Models
{
	public class Match : ModelBase
	{
		#region members
		private int positionKey;
		#endregion

		#region ctors
		public Match()
			:base()
		{
		}

		public Match(int positionKey, Player player1, Player player2)
			:base()
		{
			this.PositionKey = positionKey;
			this.Player1 = player1;
			this.Player2 = player2;
		}

		public Match(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.PositionKey = Int32.Parse(itemArray[4]);
			this.Player1Legs = Int32.Parse(itemArray[5]);
			this.Player2Legs = Int32.Parse(itemArray[6]);
		}
		#endregion

		#region properties
		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
		public int PositionKey 
		{ 
			get 
			{ 
				return this.positionKey; 
			} 
			set 
			{ 
				this.positionKey = value;
				this.DisplayName = this.positionKey.ToString();
			} 
		}
		public int Player1Legs { get; set; }
		public int Player2Legs { get; set; }
		#endregion

		#region public methods
		public Player GetWinner()
		{
			if (Player1Legs == 0 && Player2Legs == 0)
				return null;
			if (Player1Legs >= Player2Legs)
				return this.Player1;
			return this.Player2;
		}

		public Player GetLoser()
		{
			if (Player1Legs == 0 && Player2Legs == 0)
				return null;
			if (Player1Legs >= Player2Legs)
				return this.Player2;
			return this.Player1;
		}
		#endregion
	}
}
