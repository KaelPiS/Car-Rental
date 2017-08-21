using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Core.Common.Cores
{
    public class ObjectNotification:INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _PropertyChanged;
        protected List<PropertyChangedEventHandler> PropertyChangedSubcribers = new List<PropertyChangedEventHandler>();
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!PropertyChangedSubcribers.Contains(value))
                {
                    _PropertyChanged += value;
                    PropertyChangedSubcribers.Add(value);
                }
            }

            remove
            {
                _PropertyChanged -= value;
                PropertyChangedSubcribers.Remove(value);
            }
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> PropertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(PropertyExpression);
            OnPropertyChanged(propertyName);
        }
    }
}
