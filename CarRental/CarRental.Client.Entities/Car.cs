using Core.Common.Cores;

namespace CarRental.Client.Entities
{
    public class Car:ObjectBase
    {
        private string _CarID;

        public string CarID
        {
            get
            {
                return _CarID;
            }

            set
            {
                if (_CarID != value)
                {
                    _CarID = value;
                    OnPropertyChanged("CarID");
                }
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string Color
        {
            get
            {
                return _Color;
            }

            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    OnPropertyChanged("Color");
                }
            }
        }

        public int Year
        {
            get
            {
                return _Year;
            }

            set
            {
                if (_Year != value)
                {
                    _Year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        public decimal RentalPrice
        {
            get
            {
                return _RentalPrice;
            }

            set
            {
                if (_RentalPrice != value)
                {
                    _RentalPrice = value;
                    OnPropertyChanged("RentalPrice");
                }
            }
        }

        public bool CurrentlyRented
        {
            get
            {
                return _CurrentlyRented;
            }

            set
            {
                if (_CurrentlyRented != value)
                {
                    _CurrentlyRented = value;
                    OnPropertyChanged("CurrentlyRented");
                }
            }
        }

        private string _Description;

        private string _Color;

        private int _Year;

        private decimal _RentalPrice;

        private bool _CurrentlyRented;
    }
}
