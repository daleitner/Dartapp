using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using DartApp.Models;
using Base;
using DartApp.Database;

namespace DartApp.Database.ModelViews
{
	public class PlayerViewModel : ViewModelBase
	{
		#region members
		private Player player = null;
		private Uri picsPath = null;
		private BitmapImage imageSource = null;
		#endregion

		#region ctor
		public PlayerViewModel(Player p)
		{
			this.player = p;
			if (this.player != null)
			{
				this.picsPath = new Uri(Path.GetFullPath("pics"));
				if (File.Exists(new Uri(picsPath, ".\\pics\\" + this.player.ImageName).LocalPath))
				{
					this.imageSource = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.player.ImageName));
				}
			}
		}
		#endregion

		#region properties

		public string VorName
		{
			get
			{
				return this.player.VorName;
			}
			private set
			{
				this.player.VorName = value;
			}
		}

		public string NachName
		{
			get
			{
				return this.player.NachName;
			}
			private set
			{
				this.player.NachName = value;
			}
		}

		public DateTime Geb
		{
			get
			{
				return this.player.Geb;
			}
			private set
			{
				this.player.Geb = value;
			}
		}

		public BitmapImage ImageSource
		{
			get
			{
				return this.imageSource;
			}
			set
			{
				this.imageSource = value;
				OnPropertyChanged("ImageSource");
			}
		}

		#endregion

		#region private methods

		private void UpdatePicture()
		{
			if (this.player != null && File.Exists(new Uri(picsPath, ".\\pics\\" + this.player.ImageName).LocalPath))
			{
				ImageSource = new BitmapImage(new Uri(picsPath, ".\\pics\\" + this.player.ImageName));
			}
			else
			{
				ImageSource = null;
			}
		}

		#endregion

		#region public methods

		#endregion
	}
}