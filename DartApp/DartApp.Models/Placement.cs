﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace DartApp.Models
{
	public class Placement : ModelBase
	{
		public Placement(int position, Player player )
		{
			this.Player = player;
			this.Position = position;
		}

		public Player Player { get; set; }
		public int Position { get; set; }
	}
}
