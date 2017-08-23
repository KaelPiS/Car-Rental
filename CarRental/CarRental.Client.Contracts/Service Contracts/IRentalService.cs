using CarRental.Client.Entities;
using CarRental.Common.Exceptions;
using Core.Common.Contract;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
namespace CarRental.Client.Contracts
{
    [ServiceContract]
    public interface IRentalService : IServiceContract
    {
        [OperationContract(Name = "RentCarToCustomerImmediately")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental RentCarToCustomer(string loginEmail, int carID, DateTime dateDue);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental RentCarToCustomer(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AcceptCarReturn(int carID);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        IEnumerable<Rental> GetRentalHistory(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation GetReservation(int reservationID);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Reservation MakeReservation(string loginEmail, int carID, DateTime rentalDate, DateTime returnDate);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ExecuteRentalFromReservation(int reservationID);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void CancelReservation(int reservationID);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerReservationData[] GetCurrentReservations();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerReservationData[] GetCustomerReservations(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental GetRental(int rentalID);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerRentalData[] GetCurrentRentals();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation[] GetDeadReservations();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        bool IsCarCurrentlyRented(int carID);

        // Async operations

        [OperationContract(Name = "RentCarToCustomerImmediately")]
        Task <Rental> RentCarToCustomerAsync(string loginEmail, int carID, DateTime dateDue);


        [OperationContract]
        Task<Rental> RentCarToCustomerAsync(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        Task AcceptCarReturnAsync(int carID);

        [OperationContract]

        Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail);

        [OperationContract]
        Task<Reservation> GetReservationAsync(int reservationID);

        [OperationContract]
        Task<Reservation> MakeReservationAsync(string loginEmail, int carID, DateTime rentalDate, DateTime returnDate);

        [OperationContract]
        Task ExecuteRentalFromReservationAsync(int reservationID);

        [OperationContract]

        Task CancelReservationAsync(int reservationID);

        [OperationContract]
        Task<CustomerReservationData[]> GetCurrentReservationsAsync();

        [OperationContract]
        Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail);

        [OperationContract]
        Task<Rental> GetRentalAsync(int rentalID);

        [OperationContract]
        Task<CustomerRentalData[]> GetCurrentRentalsAsync();

        [OperationContract]
        Task<Reservation[]> GetDeadReservationsAsync();

        [OperationContract]
        Task<bool> IsCarCurrentlyRentedAsync(int carID);

    }
}
