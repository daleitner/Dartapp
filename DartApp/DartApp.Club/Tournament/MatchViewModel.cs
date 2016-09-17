using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartApp.Club.Tournament
{
	public class MatchViewModel : ViewModelBase
	{
		private Match match;
		public MatchViewModel(Match match)
		{
			this.match = match;
		}
	}
}
