using CarRental.Business.Contracts;
using Core.Common.Contract;
using Core.Common.Cores;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data.Contract.Repository_Interfaces;
using Core.Common.Exceptions;
using CarRental.Business.Common;
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Managers.Managers
{
    // By default, without specific ServiceBehaviour, WCF will instantiate this service in per-session mode, which means 
    // the lifetime of the service will be equal to the lifetime of the proxy

    // These settings below will turn this service into something called PerCall service, which means every call happen in a new instance 
    // and after the call complete, the service will be disposed.

    // Because every call will be handled in different instance of service, we can set the Concurrency into Multiple without 
    // worry about multi-threading problems
    
    
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode =ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;
        public InventoryManager()
        {      
        }

        // This for testing purpose only, WCF not even care about this constructor, all it cares about is the default constructor
        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAllCars()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                // Just simply walk through the list of cars and set the currentRented property, because I told Entity Framework
                // to ignore the currentRented property so it will not be mapped.
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                IEnumerable<Car> cars = carRepository.Get();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCar();
                foreach (Car car in cars)
                {
                    Rental rentedCar = rentedCars.Where(item => item.CarID == car.CarID).FirstOrDefault();
                    car.CurrentlyRented = (rentedCar != null);
                }
                return cars.ToArray();
            });
        }

        // WCF will authorize this operation only for users that are in the "CarRentalAdminRole" windows group. In the case of desktop 
        // app, it will be the actual user behind the firewall. In the case of web app, it will be the IIS
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                Car carEntity = carRepository.Get(carId);
                if (carEntity == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Car with ID of {0} is not found in the database", carId));
                    // Because of the physical separation between server and client, the client can not automatically catch normal exception
                    // we need to wrap this exception into SOAP fault, which will get transmitted through SOAP
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                return carEntity;
            });
           
        }

        // Just instantiating the carRepository using _DataRepositoryFactory, depend on the existence of CarID property, it will 
        // add or update end then return the updatedEntity

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                Car updatedEntity = null;
                if (car.CarID == 0)
                    updatedEntity = carRepository.Add(car);
                else
                    updatedEntity = carRepository.Update(car);
                return updatedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                carRepository.Remove(carId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                IEnumerable<Car> allCars = carRepository.Get();
                IEnumerable<Rental> allRentals = rentalRepository.GetCurrentlyRentedCar();
                IEnumerable<Reservation> allReservations = reservationRepository.Get();

                List<Car> availableCars = new List<Car>();
                foreach (Car car  in allCars)
                {
                    if (carRentalEngine.IsCarAvailableForRental(car.CarID, pickupDate, returnDate, allRentals, allReservations))
                        availableCars.Add(car);
                }
                return availableCars.ToArray();
            });
        }
    }
}
