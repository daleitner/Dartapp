using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Base;

namespace DartApp.Controls
{
    public class ItemSelectionViewModel : ViewModelBase
    {
        #region members
        private ObservableCollection<object> allObjects = null;
        private ObservableCollection<object> selectedObjects = null;
        private object allObjectsSelection = null;
        private object selectedObjectsSelection = null;
        private string allHeader = "";
        private string selectedHeader = "";
        private RelayCommand selectCommand = null;
        private RelayCommand deselectCommand = null;
		public delegate void ItemSelectedEventHandler(object selectedItem, List<object> selectedItems);
		public event ItemSelectedEventHandler ItemSelected = null;
		public event ItemSelectedEventHandler ItemDeselected = null;
        #endregion

        #region ctors
        public ItemSelectionViewModel(ObservableCollection<object> allObjects, ObservableCollection<object> selectedObjects, string allheader, string selectedheader)
        {
            this.allObjects = allObjects;
            this.selectedObjects = selectedObjects;
            this.allHeader = allheader;
            this.selectedHeader = selectedheader;
        }

        public ItemSelectionViewModel(ItemSelectionViewModel item)
		{
			if (item != null)
			{
				this.allObjects = new ObservableCollection<object>(item.AllObjects);
				this.selectedObjects = new ObservableCollection<object>(item.SelectedObjects);
				this.allHeader = item.AllHeader;
				this.selectedHeader = item.SelectedHeader;
			}
        }
        #endregion

        #region properties
        public ObservableCollection<object> AllObjects
        {
            get
            {
                return this.allObjects;
            }
            set
            {
                this.allObjects = value;
                OnPropertyChanged("AllObjects");
            }
        }

        public ObservableCollection<object> SelectedObjects
        {
            get
            {
                return this.selectedObjects;
            }
            set
            {
                this.selectedObjects = value;
                OnPropertyChanged("SelectedObjects");
            }
        }

        public object AllObjectsSelection
        {
            get
            {
                return this.allObjectsSelection;
            }
            set
            {
                this.allObjectsSelection = value;
                OnPropertyChanged("AllObjectsSelection");
            }
        }

        public object SelectedObjectsSelection
        {
            get
            {
                return this.selectedObjectsSelection;
            }
            set
            {
                this.selectedObjectsSelection = value;
                OnPropertyChanged("SelectedObjectsSelection");
            }
        }

        public string AllHeader
        {
            get
            {
                return this.allHeader;
            }
            set
            {
                this.allHeader = value;
                OnPropertyChanged("AllHeader");
            }
        }

        public string SelectedHeader
        {
            get
            {
                return this.selectedHeader;
            }
            set
            {
                this.selectedHeader = value;
                OnPropertyChanged("SelectedHeader");
            }
        }

        public ICommand SelectCommand
        {
            get
            {
                if (this.selectCommand == null)
                {
                    this.selectCommand = new RelayCommand(
                        param => Select(),
						param => CanSelect()
                            );
                }
                return this.selectCommand;
            }
        }

        public ICommand DeselectCommand
        {
            get
            {
                if (this.deselectCommand == null)
                {
                    this.deselectCommand = new RelayCommand(
                        param => Deselect(),
						param => CanDeselect()
                            );
                }
                return this.deselectCommand;
            }
        }
        #endregion

        #region buttonhandler
        public void Select()
        {
	        if (this.AllObjectsSelection == null)
				return;

	        this.SelectedObjects.Add(this.AllObjectsSelection);
	        this.AllObjects.Remove(this.AllObjectsSelection);

	        if(ItemSelected != null)
				ItemSelected(this.AllObjectsSelection, this.SelectedObjects.ToList());

	        if (this.AllObjects.Count > 0)
		        this.AllObjectsSelection = this.AllObjects[0];
        }

	    private bool CanSelect()
	    {
		    return this.AllObjectsSelection != null;
	    }

        public void Deselect()
        {
	        if (this.SelectedObjectsSelection == null)
				return;

	        this.AllObjects.Add(this.SelectedObjectsSelection);
	        this.AllObjects = new ObservableCollection<object>(this.AllObjects.OrderBy(x => x.ToString()));

	        this.SelectedObjects.Remove(this.SelectedObjectsSelection);

	        if(ItemDeselected != null)
				ItemDeselected(this.SelectedObjectsSelection, this.AllObjects.ToList());

	        if (this.SelectedObjects.Count > 0)
		        this.SelectedObjectsSelection = this.SelectedObjects[0];
        }

	    private bool CanDeselect()
	    {
			return this.SelectedObjectsSelection != null;
	    }
        #endregion
    }
}
