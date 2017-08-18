using CarRental.Business.Common;
using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Business_Engines
{
    // The Business Engine make the behaviour to be reuseable, make the business testable
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine:ICarRentalEngine
    {
        public bool IsCarAvailableForRental(int carID, DateTime pickupDate, DateTime returnDate, 
            IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            bool isAvailable = true;
            Reservation reservation = reservedCars.Where(item => item.CarID == carID).FirstOrDefault();
            if (reservation!=null && ((pickupDate >= reservation.RentalDate && pickupDate<=reservation.ReturnDate) || 
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                isAvailable = false;
            }

            if (isAvailable)
            {
                Rental rental = rentedCars.Where(item => item.CarID == carID).FirstOrDefault();
                if (rental != null && (pickupDate <= rental.DateDue))
                    isAvailable = false;
            }


            return isAvailable;
        }
    }


}
