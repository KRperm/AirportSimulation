using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AirportSimulation.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args);
        }

        protected void SetProperty<T>(ref T propertyValue, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (Equals(propertyValue, newValue)) return;
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
