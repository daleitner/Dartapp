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

		//public Player Player
		//{
		//	get
		//	{
		//		return this.player;
		//	}
		//	set
		//	{
		//		this.player = value;
		//		OnPropertyChanged("Player");
		//		UpdatePicture();
		//	}
		//}

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

		//private void EditButtonClick()
		//{
		//	addplayerwindow = new AddPlayerWindow();
		//	AddPlayerViewModel viewmodel = new AddPlayerViewModel(Player);
		//	viewmodel.ButtonClicked += new AddPlayerViewModel.ButtonClickedEventHandler(E_EditPlayer);
		//	addplayerwindow.DataContext = viewmodel;
		//	addplayerwindow.ShowDialog();
		//}

		//private void DeleteButtonClick()
		//{
		//	MessageBoxResult ret = MessageBox.Show("Willst du " + Player.ToString() + " wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

		//	if (ret.ToString() == "Yes")
		//	{
		//		Delete();
		//	}
		//}

		//private void NewButtonClick()
		//{
		//	AddNewPlayer();
		//}

		//private void Delete()
		//{
		//	//ObservableCollection<Player> AllPlayers = query.GetPlayers();
		//	//int index = AllPlayers.IndexOf(Player);
		//	//if (index != -1)
		//	//{
		//		this.dataBase.DeletePlayer(Player);
		//		Log(Player.VorName + " " + Player.NachName + " gelöscht!");
		//		FireListUpdated(Operation.Delete, Player);
		//	//}
		//	//else
		//	//{
		//	//    Log("Spieler nicht gefunden!");
		//	//}
		//}


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

		//private void UpdateItemList()
		//{
		//	Items = new ObservableCollection<ListItemViewModel>();
		//	ObservableCollection<Team> helpteams = this.dataBase.GetTeams(this.Player);
		//	if (helpteams != null)
		//	{
		//		foreach (Team t in helpteams)
		//		{
		//			ListItemViewModel help = new ListItemViewModel(t);
		//			help.LeftClick += new ListItemViewModel.LeftClickEventHandler(E_ShowTeamInfo);
		//			this.Items.Add(help);
		//		}
		//	}
		//}
		#endregion

		#region public methods

		//public override void E_ButtonClick(string name, Operation op)
		//{
		//	switch (op)
		//	{
		//		case Operation.New: NewButtonClick(); break;
		//		case Operation.Delete: DeleteButtonClick(); break;
		//		case Operation.Update: EditButtonClick(); break;
		//	}
		//}

		//public void AddNewPlayer()
		//{
		//	addplayerwindow = new AddPlayerWindow();
		//	AddPlayerViewModel viewmodel = new AddPlayerViewModel();
		//	viewmodel.ButtonClicked += new AddPlayerViewModel.ButtonClickedEventHandler(E_AddNewPlayer);
		//	addplayerwindow.DataContext = viewmodel;
		//	addplayerwindow.ShowDialog();
		//}

		//public void SelectedItemChanged(object item)
		//{
		//	Player = (Player)item;
		//}
		#endregion

		#region eventhandler


		//void E_EditPlayer(Player modifiedPlayer)
		//{
		//	addplayerwindow.Close();
		//	if (modifiedPlayer != null)
		//	{
		//		this.dataBase.UpdatePlayer(Player, modifiedPlayer);
		//		FireListUpdated(Operation.Update, modifiedPlayer);
		//	}
		//}

		//void E_AddNewPlayer(Player newPlayer)
		//{
		//	if (newPlayer != null)
		//	{
		//		bool check = true;
		//		ObservableCollection<Player> AllPlayers = this.dataBase.GetObjects<Player>();
		//		if (AllPlayers != null)
		//		{
		//			foreach (Player p in AllPlayers)
		//			{
		//				if (p.ToString() == newPlayer.ToString())
		//				{
		//					check = false;
		//					break;
		//				}
		//			}
		//		}
		//		if (check)
		//		{
		//			this.dataBase.InsertPlayer(newPlayer);
		//			addplayerwindow.Close();
		//			FireListUpdated(Operation.New, newPlayer);
		//		}
		//		else
		//		{
		//			MessageBox.Show("Der Spieler existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		//			Log("Der Spieler " + newPlayer.ToString() + " existiert bereits!");
		//		}
		//	}
		//	else
		//	{
		//		addplayerwindow.Close();
		//	}
		//}

		//void E_ShowTeamInfo(object team)
		//{
		//	Team SelectedTeam = (Team)team;
		//	FireItemClicked(ContentEnum.Mannschaft, SelectedTeam);
		//}
		#endregion
	}
}