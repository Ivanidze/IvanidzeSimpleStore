using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SimpleStore.Domain;
using SimpleStore.ViewModels.Stuff;

namespace SimpleStore.ViewModels
{
    public class WareGroupTreeViewWrapper : INotifyPropertyChanged
    {
        private readonly WareGroup _wareGroup;
        private WareGroupTreeViewWrapper _parent;
        private ObservableCollection<WareGroupTreeViewWrapper> _children;
        public Action<WareGroupTreeViewWrapper> OnSelect;
        public Action<WareGroupTreeViewWrapper> OnEdit;
        
        public WareGroupTreeViewWrapper(WareGroup wareGroup, Action<WareGroupTreeViewWrapper> onEdit, Action<WareGroupTreeViewWrapper> onSelect)
            : this(wareGroup, onEdit, onSelect, null)
        {
        }

        public WareGroupTreeViewWrapper(WareGroup wareGroup, Action<WareGroupTreeViewWrapper> onEdit, Action<WareGroupTreeViewWrapper> onSelect, WareGroupTreeViewWrapper parent)
        {
            _wareGroup = wareGroup;
            _parent = parent;
            OnEdit = onEdit;
            OnSelect = onSelect;
            
            _children = new ObservableCollection<WareGroupTreeViewWrapper>(
                (from child in _wareGroup.Children
                 select new WareGroupTreeViewWrapper(child, onEdit, onSelect, this))
                 );
            
        }
        public void RefreshChildren()
        {
            _children = new ObservableCollection<WareGroupTreeViewWrapper>(
                (from child in _wareGroup.Children
                 select new WareGroupTreeViewWrapper(child, OnEdit, OnSelect, this))
                 );
        }

        public ObservableCollection<WareGroupTreeViewWrapper> Children
        {
            get { return _children; }
        }




        
        public WareGroupTreeViewWrapper Parent
        {
            get { return _parent; }
            set { 
                if (_parent!=null)
                {
                    _parent.WareGroup.RemoveChild(this._wareGroup);
                    _parent.Children.Remove(this);
                }
                _parent = value;
                if (_parent != null)
                {
                    _parent.WareGroup.AddChild(this._wareGroup);
                    _parent.Children.Add(this);
                }

                OnPropertyChanged("Parent");
            }

        }
        private ICommand _selectCurrentForEdit;
        public ICommand SelectCurrentForEdit
        {
            get
            {
                if (_selectCurrentForEdit == null)
                {
                    _selectCurrentForEdit = new RelayCommand(
                        (o => { 
                            if (OnEdit!=null) 
                            OnEdit(this); })
                            , null);

                }
                return _selectCurrentForEdit;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { if (value!=_isSelected)
            {
                  if (value&&OnSelect!=null)
                    OnSelect(this);
                _isSelected = value;
                 OnPropertyChanged("IsSelected");

            }
            }
        }
        
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }          
            set{ 
                if (_isExpanded!=value)
                {
                    _isExpanded = value;
                        OnPropertyChanged("IsExpanded");
                }
            }
        }
        public WareGroup WareGroup
        {
            get { return _wareGroup; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string value)
        {
            if (PropertyChanged!=null)
                PropertyChanged(this,new PropertyChangedEventArgs(value));
        }
    }
}
