using CarRental.Business.Entities;
using Core.Common.Contract;
using System;
using System.Collections.Generic;

namespace CarRental.Business.Common
{
    public interface ICarRentalEngine:IBusinessEngine
    {
        bool IsCarAvailableForRental(int carID, DateTime pickupDate, DateTime returnDate,
           IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars);

        bool IsCarCurrentlyRented(int carID, int accountID);

        bool IsCarCurrentlyRented(int carID);

        Rental RentCarToCustomer(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue);
    }
}
