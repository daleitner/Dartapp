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
		private string player1 = "";
		private string player2 = "";
		private Match match;
		private RelayCommand okCommand = null;
		public delegate void MatchChangedEventHandler(Match match);
		public event MatchChangedEventHandler MatchChanged = null;
		public MatchViewModel(Match match)
		{
			this.match = match;
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
				return this.match.Player1Legs.ToString();
			}
			set 
			{
				this.match.Player1Legs = Int32.Parse(value);
				OnPropertyChanged("Player1Legs");
			}
		}

		public string Player2Legs
		{
			get
			{
				return this.match.Player2Legs.ToString();
			}
			set
			{
				this.match.Player2Legs = Int32.Parse(value);
				OnPropertyChanged("Player2Legs");
			}
		}

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
			if (this.MatchChanged != null)
				this.MatchChanged(this.match);
		}

		private bool CanClick()
		{
			return this.match.Player1Legs != this.match.Player2Legs;
		}

		internal void Refresh()
		{
			var picsPath = new Uri(Path.GetFullPath("pics"));
			if (this.match.Player1 != null)
			{
				this.Player1 = this.match.Player1.VorName;
				if (this.match.Player1.VorName == "FL")
					this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\freilos.jpg"));
				else if (File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player1.ImageName).LocalPath))
					this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player1.ImageName));
				else
					this.Player1Image = new BitmapImage(new Uri(picsPath, ".\\pics\\default.jpg"));
			}
			if (this.match.Player2 != null)
			{
				this.Player2 = this.match.Player2.VorName;
				if (this.match.Player2.VorName == "FL")
					this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\freilos.jpg"));
				else if (File.Exists(new Uri(picsPath, ".\\pics\\" + this.match.Player2.ImageName).LocalPath))
					this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.match.Player2.ImageName));
				else
					this.Player2Image = new BitmapImage(new Uri(picsPath, ".\\pics\\default.jpg"));
			}
		}
	}
}
