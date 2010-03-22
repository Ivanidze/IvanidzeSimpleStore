using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Infrasturcture;
using SimpleStore.Domain;
using SimpleStore.Domain.Model;
using System.Windows.Input;
using SimpleStore.ViewModels.Stuff;
using System.Collections.Specialized;

namespace SimpleStore.ViewModels
{

    public class WareGroupViewModel : INotifyPropertyChanged
    {
        private readonly IWareGroupModel _wareGroupModel;
        private readonly IViewFactory _viewFactory;
        private IList<WareGroup> _rootWareGroups;
        private ObservableCollection<WareGroupTreeViewWrapper> _rootedWrappers;
        private ObservableCollection<EditWareGroupViewModel> _workspaces;
        private WareGroupTreeViewWrapper _editWareGroup;
            
        
        private WareGroupTreeViewWrapper _currentSelectedWareGroup;
        private ICommand _selectCurrentForEdit;
        private ICommand _deleteSelected;
        private ICommand _editWareGroupCommand;
        private ICommand _addWareGroupCommand;
        
        
        
        private string _searchText = String.Empty;
        private ICommand _searchCommand;
        private IEnumerator<WareGroupTreeViewWrapper> _matchingWareGroupEnumerator;


        public WareGroupViewModel(IWareGroupModel wareGroupModel, IViewFactory viewFactory)
        {
            _wareGroupModel = wareGroupModel;
            _viewFactory = viewFactory;
            _rootWareGroups = _wareGroupModel.GetAllRootWareGroups();

            _rootedWrappers = new ObservableCollection<WareGroupTreeViewWrapper>(
                (from root in _rootWareGroups
                 select new WareGroupTreeViewWrapper(root, (o => EditSelected()), (o => CurrentSelectedWareGroup = o))
                            ));

        }
        public ObservableCollection<WareGroupTreeViewWrapper> WareGroups
        {
            get
            {
                return _rootedWrappers;
            }
            set
            {
                _rootedWrappers = value;
                OnPropertyChanged("WareGroups");
            }
        }
        public WareGroupTreeViewWrapper CurrentSelectedWareGroup
        {
            get { return _currentSelectedWareGroup; }
            set { _currentSelectedWareGroup = value;
            OnPropertyChanged("CurrentSelectedWareGroup");
            }
        }
        public ICommand EditWareGroupCommand
        {
            get
            {
                if (_editWareGroupCommand == null)
                    _editWareGroupCommand = new RelayCommand(
                        o => EditSelectedWareGroup(),
                        o => (CurrentSelectedWareGroup != null && !Workspaces.Any(ae => ae.WareGroup == CurrentSelectedWareGroup)));
                return _editWareGroupCommand;
            }
        }
        public ICommand AddWareGroupCommand
        {
            get
            {
                if (_addWareGroupCommand == null)
                    _addWareGroupCommand = new RelayCommand(
                        AddNewWareGroup
                        );
                return _addWareGroupCommand;
            }
        }
        public void EditSelected()
        {
            if (CurrentSelectedWareGroup != null && !Workspaces.Any(ae => ae.WareGroup == CurrentSelectedWareGroup))
                EditSelectedWareGroup();
                
                
                        
                
            
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == _searchText)
                    return;

                _searchText = value;

                _matchingWareGroupEnumerator = null;
            }
        }
        private void AddNewWareGroup()
        {
            var newWareGroup = _wareGroupModel.CreateWareGroup();
            var newWrapper = new WareGroupTreeViewWrapper(newWareGroup, (e => EditWareGroup = e),
                                                          (e => CurrentSelectedWareGroup = e), CurrentSelectedWareGroup);
            if (CurrentSelectedWareGroup == null)
            {
                WareGroups.Add(newWrapper);
            }
            else
            {
                newWareGroup.Parent = CurrentSelectedWareGroup.WareGroup;
                CurrentSelectedWareGroup.Children.Add(newWrapper);
            }
            CurrentSelectedWareGroup = newWrapper;
            EditSelected();
            
        }

        private void SetActiveWorkspace(EditWareGroupViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        private void EditSelectedWareGroup()
        {
            var newWp = _viewFactory.ResolveViewModel<EditWareGroupViewModel>();
            newWp.SetUp(() => CurrentSelectedWareGroup, _wareGroupModel);
            Workspaces.Add(newWp);
            SetActiveWorkspace(newWp);
            
        }
        private void DeleteCurrent()
        {
            var DeletedWareGroupWrapper = CurrentSelectedWareGroup;
            if (DeletedWareGroupWrapper.WareGroup.Parent == null) WareGroups.Remove(DeletedWareGroupWrapper);
            else {
                DeletedWareGroupWrapper.WareGroup.Parent.RemoveChild(DeletedWareGroupWrapper.WareGroup);
                DeletedWareGroupWrapper.Parent.Children.Remove(DeletedWareGroupWrapper); 
            }
            _wareGroupModel.DeleteWareGroup(DeletedWareGroupWrapper.WareGroup);
            CurrentSelectedWareGroup.IsSelected = false;
            CurrentSelectedWareGroup = null;
            
        }

        public ICommand DeleteSelected
        {
            get { if (_deleteSelected==null)
            {
                _deleteSelected = new RelayCommand(DeleteCurrent

                                                       
                    ,()=> CurrentSelectedWareGroup != null&&CurrentSelectedWareGroup.Children.Count==0);
            }
                return _deleteSelected;
            }
        }
        public ICommand SelectCurrentForEdit
        {
            get { if (_selectCurrentForEdit==null)
            {
                _selectCurrentForEdit = new RelayCommand((o => EditWareGroup = CurrentSelectedWareGroup));
                
            }
                return _selectCurrentForEdit;
            }

        }
        public WareGroupTreeViewWrapper EditWareGroup
        {
            get { return _editWareGroup; }
            set { 
                _editWareGroup = value;
                OnPropertyChanged("EditWareGroup");
            }
        }
        public ObservableCollection<EditWareGroupViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces= new ObservableCollection<EditWareGroupViewModel>();
                    _workspaces.CollectionChanged += (sender, args) =>
                                                         {
                                                             if (args.Action == NotifyCollectionChangedAction.Add)
                                                                 foreach (
                                                                     var wp in
                                                                         args.NewItems.OfType<EditWareGroupViewModel>())
                                                                 {
                                                                     wp.RequestClose += EditWareGroupRequestClose;
                                                                     
                                                                 }
                                                             if (args.Action == NotifyCollectionChangedAction.Remove)
                                                                 foreach (
                                                                     var wp in
                                                                         args.OldItems.OfType<EditWareGroupViewModel>())
                                                                 {
                                                                     wp.RequestClose -= EditWareGroupRequestClose;
                                                                 }
                                                         };
                }
                return _workspaces;
            }
        }
        private IAsyncCommandWithResult<object, IList<WareGroup>> _refreshWareGroups;
        public IAsyncCommandWithResult<object ,IList<WareGroup>> RefreshWareGroups
        {
            get
            {
                if (_refreshWareGroups == null)
                {
                    _refreshWareGroups =
                        new AsyncCommandWithResult<object, IList<WareGroup>>(o => _wareGroupModel.GetAllRootWareGroups())
                            {
                                BlockInteraction = true,
                                
                                Completed = (o,wareGroups)=>
                                                {
                                                    _rootWareGroups = wareGroups;
                                                    _rootedWrappers = new ObservableCollection<WareGroupTreeViewWrapper>(
                                                    (from root in _rootWareGroups
                                                    select new WareGroupTreeViewWrapper(root, (e => EditWareGroup = e), (e => CurrentSelectedWareGroup = e))
                                                    ));
                                                                                }

                            };
                }
                return _refreshWareGroups;
            }
        }
        private void EditWareGroupRequestClose(object sender, EventArgs e)
        {
            var editAlbumViewModel = (EditWareGroupViewModel)sender;
            
            Workspaces.Remove(editAlbumViewModel);
            if (editAlbumViewModel.WareGroup.WareGroup.Id <= 0)
            {
                CurrentSelectedWareGroup.IsSelected = false;
                CurrentSelectedWareGroup = null;
                if (editAlbumViewModel.WareGroup.Parent == null) WareGroups.Remove(editAlbumViewModel.WareGroup);
                else editAlbumViewModel.WareGroup.Parent.Children.Remove(editAlbumViewModel.WareGroup);
                    
            }

                
        }
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand(PerformSearch);
                return _searchCommand; }

        }
        void PerformSearch()
        {
            if (_matchingWareGroupEnumerator == null || !_matchingWareGroupEnumerator.MoveNext())
                this.VerifyMatchingPeopleEnumerator();

            var person = _matchingWareGroupEnumerator.Current;

            if (person == null)
                return;

            // Ensure that this person is in view.
            if (person.Parent != null)
                person.Parent.IsExpanded = true;

            person.IsSelected = true;
        }

        void VerifyMatchingPeopleEnumerator()
        {
           


            foreach (var roots in WareGroups)
            {
                var matches = this.FindMatches(_searchText, roots);
                _matchingWareGroupEnumerator = matches.GetEnumerator();
            }
            

                

                if (!_matchingWareGroupEnumerator.MoveNext())
                {
                }
            
        }

        IEnumerable<WareGroupTreeViewWrapper> FindMatches(string searchText, WareGroupTreeViewWrapper person)
        {
            if (person.WareGroup.Name.Contains(searchText))
                yield return person;

            foreach (WareGroupTreeViewWrapper child in person.Children)
                foreach (WareGroupTreeViewWrapper match in this.FindMatches(searchText, child))
                    yield return match;
        }


        public virtual event EventHandler RequestClose;
        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null) handler(this, new EventArgs());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
