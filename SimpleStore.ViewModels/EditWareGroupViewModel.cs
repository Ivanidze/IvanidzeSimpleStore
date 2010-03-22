using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using SimpleStore.Domain;
using SimpleStore.Domain.Model;
using System.Windows.Input;
using SimpleStore.ViewModels.Stuff;

namespace SimpleStore.ViewModels
{
    public class EditWareGroupViewModel:INotifyPropertyChanged
    {
        private WareGroupTreeViewWrapper _wareGroup;
        private IWareGroupModel _wareGroupModel;
        private WareGroupTreeViewWrapper _parentBeforeSave;
        private Func<WareGroupTreeViewWrapper> _getSelected;
        private ICommand _closeCommand;
        private ICommand _saveCommand;
        private ICommand _clearParentGroup;
        private ICommand _selectParentGroup;
        public virtual void SetUp(Func<WareGroupTreeViewWrapper>getSelected,IWareGroupModel wareGroupModel)
        {
            if (getSelected == null) throw new ArgumentNullException("getSelected");
            if (wareGroupModel == null) throw new ArgumentNullException("wareGroupModel");
            var waregroup = getSelected();
            if (waregroup==null) throw new ArgumentException("Ничего не выбрано");
            _wareGroup = waregroup;
            _parentBeforeSave = _wareGroup.Parent;
            _getSelected = getSelected;
            _wareGroupModel = wareGroupModel;
        }
        public WareGroupTreeViewWrapper WareGroup
        {
            get { return _wareGroup; }
            set { _wareGroup = value;
            OnPropertyChanged("WareGroup");
            }
        }
        public string DisplayName
        {
            get {return this.With(x => this._wareGroup).With(x => x.WareGroup).Return(x => x.Name, string.Empty);}
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(o =>
                    {
                        if (WareGroup.WareGroup.Id <= 0) _wareGroupModel.SaveWareGroup(WareGroup.WareGroup);
                        _wareGroupModel.UpdateWareGroup(WareGroup.WareGroup);

                        OnRequestClose();
                    },
                                                     o => _wareGroupModel.IsValid(WareGroup.WareGroup));
                    return _saveCommand;
            }

        }
        public ICommand CloseCommand
        { get
        {
            if (_closeCommand == null)
                _closeCommand = new RelayCommand(o =>
                                                     {
                                                         _wareGroupModel.CancelWareGroup(_wareGroup.WareGroup);
                                                         _wareGroup.Parent = _parentBeforeSave;
                                                         OnRequestClose();
                                                     }, o => _wareGroupModel.IsValid(WareGroup.WareGroup));
                return _closeCommand;
        }
        }
        public ICommand ClearParentGroup
        {
            get
            {
                if (_clearParentGroup == null)
                    _clearParentGroup = new RelayCommand(o => WareGroup.WareGroup.Parent = null, o => WareGroup.WareGroup.Parent != null);
                    return _clearParentGroup;
            }

        }
        public ICommand SelectParentGroup
        {
            get
            {
                if (_selectParentGroup == null)
                    _selectParentGroup = new RelayCommand(
                        o =>
                            {
                                WareGroup.Parent = _getSelected();
                            },
                        o => _getSelected().WareGroup != null
                        );
                return _selectParentGroup;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));

        }

        public virtual event EventHandler RequestClose;
        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null) handler(this, new EventArgs());
        }
    }
}
