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

namespace CarRental.Business.Managers.Managers
{
    // By default, without specific ServiceBehaviour, WCF will instantiate this service in per-session mode, which means 
    // the lifetime of the service will be equal to the lifetime of the proxy

    // These settings below will turn this service into something called PerCall service, which means every call happen in a new instance 
    // and after the call complete, the service will be disposed.

    // Because every call will be handled in different instance of service, we can set the Concurrency into Multiple without 
    // worry about multi-threading problems
    
    // 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode =ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;
        public InventoryManager()
        {
            
        }

        // This for testing purpose only, WCF not even care about this constructor, all it cares about is the default constructor
        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public Car[] GetAllCars()
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
    
            
        }

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
    }
}
