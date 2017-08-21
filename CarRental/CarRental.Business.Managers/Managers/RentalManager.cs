using CarRental.Business.Contracts.Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using Core.Common.Contract;
using System.ComponentModel.Composition;
using System.ServiceModel;
using CarRental.Data.Contract.Repository_Interfaces;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Common;
using CarRental.Business.Common;
using CarRental.Common.Exceptions;
using CarRental.Business.Contracts.Data_Contracts;
using CarRental.Data.Contract.DTOs;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        // Just the same as Inventory Manager
        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        protected override Account LoadAuthorizationValidationAccount(string LoginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

            Account authAcc = accountRepository.GetByLogin(LoginName);

            if (authAcc== null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find this account for login name {0}", LoginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            return authAcc;
        }
        public RentalManager()
        {
        }

        // This for testing purpose only, WCF not even care about this constructor, all it cares about is the default constructor
        public RentalManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public RentalManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }


        // Just transfer the loginEmail into AccountID and get history by this AccountID
      

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();



                Account account = accountRepository.GetByLogin(loginEmail);
                if (account==null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account login for {0}", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                // This method ensure that this account is a matching account to the user that actually setting behind the browser
                ValidateAuthorization(account);

                IEnumerable<Rental> rentalHistory = rentalRepository.GetRentalHistoryByAccount(account.AccountID);

                return rentalHistory;
               
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carID, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carID, DateTime.Now, dateDue);
                    return rental;
                }

                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }

                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }

                catch ( NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carID, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carID, rentalDate, dateDue);
                    return rental;
                }

                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }

                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }

                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AcceptCarRenturn(int carID)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Rental rental = rentalRepository.GetCurrentRentalByCar(carID);

                if (rental == null)
                {
                    CarNotRentedException ex = new CarNotRentedException(string.Format("Car {0} is not currently rented", carID));
                    throw new FaultException<CarNotRentedException>(ex, ex.Message);
                }

                rental.ReturnDate = DateTime.Now;
                Rental updatedRentalEntity = rentalRepository.Update(rental);
            });
        }

        public Reservation GetReservation(int reservationID)
        {
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            Reservation reservation = reservationRepository.Get(reservationID);
            if (reservation == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("No reservation found for id '{0}", reservationID));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            return reservation;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Reservation MakeReservation(string loginEmail, int carID, DateTime rentalDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);
                if (account==null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found on login {0}", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                Reservation reservation = new Reservation()
                {
                    AccountID = account.AccountID,
                    CarID = carID,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                Reservation savedEntity = reservationRepository.Add(reservation);
                return savedEntity;
            }); 
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void ExecuteRentalFromReservation(int reservationID)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                Reservation reservation = reservationRepository.Get(reservationID);

                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Reservation {0} is not found", reservationID));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                Account account = accountRepository.Get(reservation.AccountID);

                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Account {0} is not found", reservation.AccountID));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(account.LoginEmail, reservation.CarID, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }

                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }

                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                reservationRepository.Remove(reservation);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void CancelReservation(int reservationID)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationID);

                if (reservation==null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation is found for ID '{0}'", reservationID));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservationID);
            });
        }

        public CustomerReservationData[] GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                IEnumerable<CustomerReservationInfo> customerReservationInfo = reservationRepository.GetCustomerReservationInfo();

                foreach(CustomerReservationInfo customerInfo in customerReservationInfo)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationID = customerInfo.Reservation.ReservationID,
                        Car = customerInfo.Car.Color + " " + customerInfo.Car.Year + " " + customerInfo.Car.Description,
                        CustomerName = customerInfo.Customer.FirstName + " " + customerInfo.Customer.LastName,
                        RentalDate = customerInfo.Reservation.RentalDate,
                        ReturnDate = customerInfo.Reservation.ReturnDate
                    });
                }
                return reservationData.ToArray();
            }); 
        }

        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                Account account = accountRepository.GetByLogin(loginEmail);
                
                if (account==null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login {0}", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountID);
                foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationID = reservationInfo.Reservation.ReservationID,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });

        }

        public Rental GetRental(int rentalID)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Rental rental = rentalRepository.Get(rentalID);

                if (rental== null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No rental record for ID '{0}'", rentalID));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return rental;
            });
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                List<CustomerRentalData> rentalData = new List<CustomerRentalData>();

                IEnumerable<CustomerRentalInfo> rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();

                foreach (CustomerRentalInfo rentalInfo in rentalInfoSet)
                {
                    rentalData.Add(new CustomerRentalData()
                    {
                        RentalID = rentalInfo.Rental.RentalID,
                        Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year + " " + rentalInfo.Car.Description,
                        CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                        DateRented = rentalInfo.Rental.DateRented,
                        ExpectedReturn = rentalInfo.Rental.DateDue
                    });
                }

                return rentalData.ToArray();
            });
        }

        public Reservation[] GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IEnumerable<Reservation> reservations = reservationRepository.GetReservationByPickupDate(DateTime.Now.AddDays(-1));

                return (reservations != null ? reservations.ToArray() : null);
            });
        }

        public bool IsCarCurrentlyRented(int carID)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                return carRentalEngine.IsCarCurrentlyRented(carID);
            });
        }

        public void AcceptCarReturn(int carID)
        {
            throw new NotImplementedException();
        }
    }
}

