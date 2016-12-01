using Base;
using DartApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DartApp.Club.Tournament
{
	public class MatchViewModel : ViewModelBase
	{
		private BitmapImage player1Image = null;
		private BitmapImage player2Image = null;
		private string player1Legs = "0";
		private string player2Legs = "0";
		private string player1 = "";
		private string player2 = "";
		private Match match;
		private RelayCommand okCommand = null;
		public delegate void MatchChangedEventHandler(Match match);
		public event MatchChangedEventHandler MatchChanged = null;
		public MatchViewModel(Match match)
		{
			this.match = match;
			this.IsEntered = false;
			if (this.match != null)
			{
				Refresh();
			}
		}

		public string Player1
		{
			get
			{
				return this.player1;
			}
			set
			{
				this.player1 = value;
				OnPropertyChanged("Player1");
			}
		}

		public string Player2
		{
			get
			{
				return this.player2;
			}
			set
			{
				this.player2 = value;
				OnPropertyChanged("Player2");
			}
		}

		public BitmapImage Player1Image 
		{
			get
			{
				return this.player1Image;
			}
			set
			{
				this.player1Image = value;
				OnPropertyChanged("Player1Image");
			}
		}

		public BitmapImage Player2Image
		{
			get
			{
				return this.player2Image;
			}
			set
			{
				this.player2Image = value;
				OnPropertyChanged("Player2Image");
			}
		}

		public string Player1Legs
		{
			get
			{
				return this.player1Legs;
			}
			set
			{
				this.player1Legs = value;
				if (!string.IsNullOrEmpty(this.player1Legs))
				{
					int help = 0;
					Int32.TryParse(this.player1Legs, out help);
					this.player1Legs = help.ToString();
				}
				OnPropertyChanged("Player1Legs");
			}
		}

		public string Player2Legs
		{
			get
			{
				return this.player2Legs;
			}
			set
			{
				this.player2Legs = value;
				if (!string.IsNullOrEmpty(this.player2Legs))
				{
					int help = 0;
					Int32.TryParse(this.player2Legs, out help);
					this.player2Legs = help.ToString();
				}
				OnPropertyChanged("Player2Legs");
			}
		}

		public bool IsEntered { get; set; }

		public ICommand OkCommand
		{
			get
			{
				if (this.okCommand == null)
				{
					this.okCommand = new RelayCommand(
						param => OkClicked(),
						param => CanClick()
					);
				}
				return this.okCommand;
			}
		}

		private void OkClicked()
		{
			this.match.Player1Legs = Int32.Parse(this.player1Legs);
			this.match.Player2Legs = Int32.Parse(this.player2Legs);
			this.IsEntered = true;
			if (this.MatchChanged != null)
				this.MatchChanged(this.match);
		}

		private bool CanClick()
		{
			return !string.IsNullOrEmpty(this.player1Legs) && !string.IsNullOrEmpty(this.player2Legs) && 
				this.player1Legs != this.player2Legs;
		}

		internal void Refresh()
		{
			var picsPath = new Uri(Path.GetFullPath("pics"));
			if (this.match.Player1 != null)
			{
				this.Player1 = this.match.Player1.VorName;
				if (this.match.Player1.VorName == "FL")
				{
					this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\freilos.jpg"));
				}
				else
				{
					if (File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player1.ImageName).LocalPath))
						this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player1.ImageName));
					else if(File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player1.VorName + "_" + this.match.Player1.NachName + ".jpg").LocalPath))
						this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player1.VorName + "_" + this.match.Player1.NachName + ".jpg"));
					else
						this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\default.jpg"));
					this.Player1 += " " + this.match.Player1.NachName;
				}
			}
			else
			{
				this.Player1 = "";
				this.Player1Image = null;
			}
			if (this.match.Player2 != null)
			{
				this.Player2 = this.match.Player2.VorName;
				if (this.match.Player2.VorName == "FL")
					this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\freilos.jpg"));
				else
				{
					if (File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player2.ImageName).LocalPath))
						this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player2.ImageName));
					else if (
						File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player2.VorName + "_" + this.match.Player2.NachName + ".jpg").LocalPath))
						this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player2.VorName + "_" + this.match.Player2.NachName + ".jpg"));
					else
						this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\default.jpg"));
					this.Player2 += " " + this.match.Player2.NachName;
				}
			}
			else
			{
				this.Player2 = "";
				this.Player2Image = null;
			}
		}

		public int GetPositionKey()
		{
			return this.match.PositionKey;
		}
	}
}
