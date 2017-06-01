using Core.Common.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Rental:ObjectBase
    {
        private int _RentalID;

        public int RentalID
        {
            get
            {
                return _RentalID;
            }

            set
            {
                if (_RentalID != value)
                {
                    _RentalID = value;
                    OnPropertyChanged(() => RentalID);
                }
            }
        }

        public int AccountID
        {
            get
            {
                return _AccountID;
            }

            set
            {
                if (_AccountID != value)
                {
                    _AccountID = value;
                    OnPropertyChanged(() => AccountID);
                }
            }
        }

        public int CarID
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
                    OnPropertyChanged(() => CarID);
                }
            }
        }

        public DateTime RentalDate
        {
            get
            {
                return _RentalDate;
            }

            set
            {
                if (_RentalDate != value)
                {
                    _RentalDate = value;
                    OnPropertyChanged(() => RentalDate);
                }
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _DueDate;
            }

            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    OnPropertyChanged(() => DueDate);
                }
            }
        }

        public DateTime? ReturnDate
        {
            get
            {
                return _ReturnDate;
            }

            set
            {
                if (_ReturnDate != value) 
                {
                    _ReturnDate = value;
                    OnPropertyChanged(() => ReturnDate);
                }
            }
        }

        private int _AccountID;
        private int _CarID;
        private DateTime _RentalDate;
        private DateTime _DueDate;
        private DateTime? _ReturnDate;
    }
}
