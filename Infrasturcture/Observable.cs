using System.ComponentModel;
namespace Infrasturcture
{
    public class Observable<T>:INotifyPropertyChanged
    {
        private T _value;
        public  Observable()
        {

        }
        public Observable(T value)
        {
            _value = value;
        }
        public  T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }
        public static implicit  operator T (Observable<T> val)
        {
            return val._value;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 
    }
}
