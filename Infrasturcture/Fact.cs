using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Infrasturcture
{
    public class Fact:INotifyPropertyChanged
    {
        private readonly Func<bool> _predicate;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Fact(INotifyPropertyChanged observable,Func<bool> predicate)
        {
            _predicate = predicate;
            observable.PropertyChanged += (sender, args) => PropertyChanged(this, new PropertyChangedEventArgs("Value"));
        }
        public bool Value
        {
            get { return _predicate(); }
        }
    }
}
