using System;
using System.ComponentModel;

namespace Meridian59.Data.Models.AdminData
{
    public class AdminObjectProperty : INotifyPropertyChanged
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        
        private string _propertyValue { get; set; }
        public string PropertyValue {
            get { return _propertyValue; }
            set
            {
                _propertyValue = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(_owner.ObjectNumber.ToString()));
            }
        }

        private AdminObject _owner;

        public AdminObjectProperty(string propertyName, string propertyType, string propertyValue, AdminObject owner, PropertyChangedEventHandler pceh)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            _propertyValue = propertyValue;
            _owner = owner;
            PropertyChanged += pceh;
        }
        public AdminObjectProperty(string propertyName, string propertyType, string propertyValue, AdminObject owner)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            _propertyValue = propertyValue;
            _owner = owner;
        }
        public AdminObjectProperty(string propertyName, string propertyType, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            _propertyValue = propertyValue;
        }
        public AdminObjectProperty()
        {
            throw new Exception("This should not happen");
        }

        public AdminObject GetOwner()
        {
            return _owner;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        
    }
}
