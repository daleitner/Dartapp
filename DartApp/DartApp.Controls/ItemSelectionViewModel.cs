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
	    private ObservableCollection<object> allSearchObjects = null;
	    private ObservableCollection<object> selectedSearchObjects = null;  
        private object allObjectsSelection = null;
        private object selectedObjectsSelection = null;
        private string allHeader = "";
        private string selectedHeader = "";
	    private string allSearch = "";
	    private string selectedSearch = "";
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
			UpdateAllSearch();
			UpdateSelectedSearch();
        }

        public ItemSelectionViewModel(ItemSelectionViewModel item)
		{
			if (item != null)
			{
				this.allObjects = new ObservableCollection<object>(item.AllObjects);
				this.selectedObjects = new ObservableCollection<object>(item.SelectedObjects);
				this.allHeader = item.AllHeader;
				this.selectedHeader = item.SelectedHeader;
				UpdateAllSearch();
				UpdateSelectedSearch();
			}
        }
        #endregion

        #region properties
        public ObservableCollection<object> AllObjects
        {
            get
            {
                return this.allSearchObjects;
            }
            set
            {
                this.allSearchObjects = value;
                OnPropertyChanged("AllObjects");
            }
        }

        public ObservableCollection<object> SelectedObjects
        {
            get
            {
                return this.selectedSearchObjects;
            }
            set
            {
                this.selectedSearchObjects = value;
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

		public string AllSearch
		{
			get
			{
				return this.allSearch;
			}
			set
			{
				this.allSearch = value;
				OnPropertyChanged("AllSearch");
				UpdateAllSearch();
			}
		}

		public string SelectedSearch
		{
			get
			{
				return this.selectedSearch;
			}
			set
			{
				this.selectedSearch = value;
				OnPropertyChanged("SelectedSearch");
				UpdateSelectedSearch();
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

		#region public methods

	    public List<object> GetAllSelectedObjects()
	    {
		    return this.selectedObjects.ToList();
	    } 
		#endregion

		#region private methods

		private void UpdateAllSearch()
	    {
		    if (string.IsNullOrEmpty(this.AllSearch))
		    {
			    this.AllObjects = new ObservableCollection<object>(this.allObjects);
			    return;
		    }
			this.AllObjects = new ObservableCollection<object>();
			foreach (var obj in this.allObjects)
		    {
			    if (obj.GetType() == typeof (ModelBase))
			    {
				    var element = (ModelBase) obj;
				    if (element.DisplayName.ToUpper().Contains(this.AllSearch.ToUpper()))
					    this.AllObjects.Add(obj);
			    }
			    else
			    {
					if (obj.ToString().ToUpper().Contains(this.AllSearch.ToUpper()))
						this.AllObjects.Add(obj);
				}
		    }
	    }

		private void UpdateSelectedSearch()
		{
			if (string.IsNullOrEmpty(this.SelectedSearch))
			{
				this.SelectedObjects = new ObservableCollection<object>(this.selectedObjects);
				return;
			}
			this.SelectedObjects = new ObservableCollection<object>();
			foreach (var obj in this.selectedObjects)
			{
				if (obj.GetType() == typeof(ModelBase))
				{
					var element = (ModelBase)obj;
					if (element.DisplayName.ToUpper().Contains(this.SelectedSearch.ToUpper()))
						this.SelectedObjects.Add(obj);
				}
				else
				{
					if (obj.ToString().ToUpper().Contains(this.SelectedSearch.ToUpper()))
						this.SelectedObjects.Add(obj);
				}
			}
		}
		#endregion
		#region buttonhandler
		public void Select()
        {
	        if (this.AllObjectsSelection == null)
				return;
			
			this.selectedObjects.Add(this.AllObjectsSelection);
	        this.allObjects.Remove(this.AllObjectsSelection);
			this.AllObjects.Remove(this.AllObjectsSelection);

	        if(ItemSelected != null)
				ItemSelected(this.AllObjectsSelection, this.selectedObjects.ToList());

	        if (this.AllObjects.Count > 0)
		        this.AllObjectsSelection = this.AllObjects[0];
			this.SelectedSearch = "";
        }

	    private bool CanSelect()
	    {
		    return this.AllObjectsSelection != null;
	    }

        public void Deselect()
        {
	        if (this.SelectedObjectsSelection == null)
				return;
			
			this.allObjects.Add(this.SelectedObjectsSelection);

	        this.selectedObjects.Remove(this.SelectedObjectsSelection);
	        this.SelectedObjects.Remove(this.SelectedObjectsSelection);

	        if(ItemDeselected != null)
				ItemDeselected(this.SelectedObjectsSelection, this.allObjects.ToList());

	        if (this.SelectedObjects.Count > 0)
		        this.SelectedObjectsSelection = this.SelectedObjects[0];
	        this.AllSearch = "";
        }

	    private bool CanDeselect()
	    {
			return this.SelectedObjectsSelection != null;
	    }
        #endregion
    }
}
