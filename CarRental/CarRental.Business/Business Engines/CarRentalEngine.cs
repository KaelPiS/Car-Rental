using CarRental.Business.Common;
using CarRental.Business.Entities;
using CarRental.Common.Exceptions;
using CarRental.Data.Contract.Repository_Interfaces;
using Core.Common.Contract;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CarRental.Business.Business_Engines
{
    // The Business Engine make the behaviour to be reuseable, make the business testable
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine:ICarRentalEngine
    {
        [ImportingConstructor]
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        IDataRepositoryFactory _DataRepositoryFactory;
        public bool IsCarAvailableForRental(int carID, DateTime pickupDate, DateTime returnDate, 
            IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            bool isAvailable = true;
            Reservation reservation = reservedCars.Where(item => item.CarID == carID).FirstOrDefault();
            if (reservation != null && ((pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
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

        public bool IsCarCurrentlyRented(int carID, int accountID)
        {
            bool isRented = false;

            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            Rental currentRental = rentalRepository.GetCurrentRentalByCar(carID);

            if (currentRental != null && currentRental.CarID == accountID)
                isRented = true;

            return isRented;
        }

        public bool IsCarCurrentlyRented(int carID)
        {
            bool isRented = false;

            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            Rental currentRental = rentalRepository.GetCurrentRentalByCar(carID);

            if (currentRental != null)
                isRented = true;

            return isRented;
        }

        public Rental RentCarToCustomer(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException(string.Format("Can not rent for date {0} yet.", rentalDate.ToShortDateString()));

            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            bool isCarRented = IsCarCurrentlyRented(carID);

            if (isCarRented)
            {
                throw new CarCurrentlyRentedException(string.Format("Car {0} is currently rented", carID));
            }

            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
                throw new NotFoundException(string.Format("No account found for login {0}", loginEmail));

            Rental rental = new Rental()
            {
                AccountID = account.AccountID,
                CarID = carID,
                DateRented = rentalDate,
                DateDue = dateDue
            };

            Rental savedEntity = rentalRepository.Add(rental);

            return savedEntity;
        }
    }


}
