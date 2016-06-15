using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using DartApp.Models;
using Base;

namespace DartApp.Database.EditDialogs
{
    public class AddPlayerViewModel : ViewModelBase
    {
        #region members
        private Player newPlayer = null;
        private string windowTitle = "";
        private BitmapImage imageSource = null;
        private RelayCommand removeCommand = null;
        private RelayCommand chooseCommand = null;
        private RelayCommand saveCommand = null;
        private RelayCommand cancelCommand = null;
        public delegate void ButtonClickedEventHandler(Player newPlayer);
        public event ButtonClickedEventHandler ButtonClicked = null;
        #endregion

        #region ctor
        public AddPlayerViewModel()
        {
            this.newPlayer = new Player();
            this.windowTitle = "New Player";
        }

        public AddPlayerViewModel(Player player)
        {
            this.newPlayer = Player.Copy(player);
            this.windowTitle = player.ToString();
            Uri picsPath = new Uri(Path.GetFullPath("pics"));
            if(player.ImageName != "" && File.Exists(new Uri(picsPath, ".\\pics\\" + player.ImageName).LocalPath))
                this.imageSource = new BitmapImage(new Uri(picsPath, ".\\pics\\" + player.ImageName));
        }
        #endregion

        #region properties
     
        public Player NewPlayer
        {
            get
            {
                return this.newPlayer;
            }
            set
            {
                this.newPlayer = value;
                OnPropertyChanged("NewPlayer");
            }
        }

        public string WindowTitle
        {
            get
            {
                return this.windowTitle;
            }
            set
            {
                this.windowTitle = value;
                OnPropertyChanged("WindowTitle");
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

        public ICommand RemoveCommand
        {
            get
            {
                if (this.removeCommand == null)
                {
                    this.removeCommand = new RelayCommand(
                        param => Remove(),
                        param => CanRemove()
                            );
                }
                return this.removeCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new RelayCommand(
                        param => Cancel()
                            );
                }
                return this.cancelCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (this.saveCommand == null)
                {
                    this.saveCommand = new RelayCommand(
                        param => Save()
                            );
                }
                return this.saveCommand;
            }
        }

        public ICommand ChooseCommand
        {
            get
            {
                if (this.chooseCommand == null)
                {
                    this.chooseCommand = new RelayCommand(
                        param => Choose()
                            );
                }
                return this.chooseCommand;
            }
        }
        #endregion

        #region buttonhandler
        private void Remove()
        {
            ImageSource = null;
            NewPlayer.ImageName = "";
        }

        private bool CanRemove()
        {
            if(ImageSource != null)
                return true;
            return false;
        }
        private void Cancel()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(null);
            }
        }

        private void Save()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(NewPlayer);
            }
        }

        private void Choose()
        {
            Uri picsPath = new Uri(Path.GetFullPath("pics"));
            var dialog = new OpenFileDialog { InitialDirectory = picsPath.LocalPath, Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif" };
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                ImageSource = new BitmapImage(new Uri(dialog.FileName));
                newPlayer.ImageName = dialog.FileName.Split('\\').Last<string>();
            }
        }
        #endregion
    }
}
