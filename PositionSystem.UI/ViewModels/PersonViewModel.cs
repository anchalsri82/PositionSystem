using PositionSystem.UI.Models;
using System;
using System.ComponentModel;

namespace PositionSystem.UI.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Person _person;
        public PersonViewModel()
        {
            _person = new Person();
        }

        #region Public members
        public string Name {
            get { return _person.Name; }
            set { _person.Name = value; OnPropertyChanged("Name"); }
        }

        public string Address
        {
            get { return _person.Address; }
            set { _person.Address = value; OnPropertyChanged("Address"); }
        }

        #region IDataErrorInfo members
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                switch (propertyName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            result = "Name is required!";
                        break;
                    case "Address":
                        if (string.IsNullOrEmpty(Address))
                            result = "Address is required";
                        break;

                }
                return result;
            }
        }
        public string Error => throw new NotImplementedException();
        #endregion
        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
