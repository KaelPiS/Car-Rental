using CarRental.Client.Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CarRental.Client.Entities;
using System.ComponentModel.Composition;
using System.Threading;
using Core.Common.ServiceModel;

namespace CarRental.Client.Proxies.Service_Proxies
{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : UserClientBase<IRentalService>, IRentalService
    {
        
        public void AcceptCarReturn(int carID) => Channel.AcceptCarReturn(carID);

        public Task AcceptCarReturnAsync(int carID) => Channel.AcceptCarReturnAsync(carID);

        public void CancelReservation(int reservationID) => Channel.CancelReservation(reservationID);

        public Task CancelReservationAsync(int reservationID) => Channel.CancelReservationAsync(reservationID);

        public void ExecuteRentalFromReservation(int reservationID) => Channel.ExecuteRentalFromReservation(reservationID);

        public Task ExecuteRentalFromReservationAsync(int reservationID) => Channel.ExecuteRentalFromReservationAsync(reservationID);

        public CustomerRentalData[] GetCurrentRentals() => Channel.GetCurrentRentals();

        public Task<CustomerRentalData[]> GetCurrentRentalsAsync() => Channel.GetCurrentRentalsAsync();

        public CustomerReservationData[] GetCurrentReservations() => Channel.GetCurrentReservations();

        public Task<CustomerReservationData[]> GetCurrentReservationsAsync() => Channel.GetCurrentReservationsAsync();

        public CustomerReservationData[] GetCustomerReservations(string loginEmail) => Channel.GetCustomerReservations(loginEmail);

        public Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail) => Channel.GetCustomerReservationsAsync(loginEmail);

        public Reservation[] GetDeadReservations() => Channel.GetDeadReservations();

        public Task<Reservation[]> GetDeadReservationsAsync() => Channel.GetDeadReservationsAsync();

        public Rental GetRental(int rentalID) => Channel.GetRental(rentalID);

        public Task<Rental> GetRentalAsync(int rentalID) => Channel.GetRentalAsync(rentalID);

        public IEnumerable<Rental> GetRentalHistory(string loginEmail) => Channel.GetRentalHistory(loginEmail);

        public Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail) => Channel.GetRentalHistoryAsync(loginEmail);

        public Reservation GetReservation(int reservationID) => Channel.GetReservation(reservationID);

        public Task<Reservation> GetReservationAsync(int reservationID) => Channel.GetReservationAsync(reservationID);

        public bool IsCarCurrentlyRented(int carID) => Channel.IsCarCurrentlyRented(carID);

        public Task<bool> IsCarCurrentlyRentedAsync(int carID) => Channel.IsCarCurrentlyRentedAsync(carID);

        public Reservation MakeReservation(string loginEmail, int carID, DateTime rentalDate, DateTime returnDate) => Channel.MakeReservation(loginEmail, carID, rentalDate, returnDate);

        public Task<Reservation> MakeReservationAsync(string loginEmail, int carID, DateTime rentalDate, DateTime returnDate) => Channel.MakeReservationAsync(loginEmail, carID, rentalDate, returnDate);

        public Rental RentCarToCustomer(string loginEmail, int carID, DateTime dateDue) => Channel.RentCarToCustomer(loginEmail, carID, dateDue);

        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carID, DateTime dateDue) => Channel.RentCarToCustomerAsync(loginEmail, carID, dateDue);

        public Rental RentCarToCustomer(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue) => Channel.RentCarToCustomer(loginEmail, carID, rentalDate, dateDue);


        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue) => Channel.RentCarToCustomerAsync(loginEmail, carID, rentalDate, dateDue);
    }
}
