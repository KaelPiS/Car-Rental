using CarRental.Client.Contracts;
using Core.Common.ServiceModel;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies.Service_Proxies
{
    // A WCF proxy class is a class that hides all the low-level code needed to establish communications with the WCF service

    // ClientBase is the class that open up a communication channel with a hosted service and establish contact with it and provide communication
    // This inheritance will tell InventoryClient to get all the operations and expose them. 
    // This inheritance, the channel property has the same operations that defined in the IInventoryService 

    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient : UserClientBase<IInventoryService>, IInventoryService
    {
        public void DeleteCar(int carID) => Channel.DeleteCar(carID);

        public Task DeleteCarAsync(int carID) => Channel.DeleteCarAsync(carID);

        public Entities.Car[] GetAllCars() => Channel.GetAllCars();

        public Task<Entities.Car[]> GetAllCarsAsync() => Channel.GetAllCarsAsync();
        public Entities.Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate) => Channel.GetAvailableCars(pickupDate, returnDate);

        public Task<Entities.Car[]> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate) => Channel.GetAvailableCarsAsync(pickupDate, returnDate);

        public Entities.Car GetCar(int carID) => Channel.GetCar(carID);

        public Task<Entities.Car> GetCarAsync(int carID) => Channel.GetCarAsync(carID);

        public Entities.Car UpdateCar(Entities.Car car) => Channel.UpdateCar(car);

        public Task<Entities.Car> UpdateCarAsync(Entities.Car car) => Channel.UpdateCarAsync(car);
    }
}
