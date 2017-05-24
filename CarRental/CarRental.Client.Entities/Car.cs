using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Car
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
                _CarID = value;
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
                _Description = value;
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
                _Color = value;
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
                _Year = value;
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
                _RentalPrice = value;
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
                _CurrentlyRented = value;
            }
        }

        private string _Description;

        private string _Color;

        private int _Year;

        private decimal _RentalPrice;

        private bool _CurrentlyRented;
    }
}
