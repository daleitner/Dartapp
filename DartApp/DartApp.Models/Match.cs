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
		private int positionKey;
		public Match()
			:base()
		{
		}

		public Match(List<string> itemArray)
		{
			this.Id = itemArray[0];
			this.PositionKey = Int32.Parse(itemArray[4]);
			this.Player1Legs = Int32.Parse(itemArray[5]);
			this.Player2Legs = Int32.Parse(itemArray[6]);
		}

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
	}
}
