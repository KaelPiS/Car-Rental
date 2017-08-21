using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.ServiceModel;

namespace CarRental.Business.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        // In order to able to rise FaultException of NotFoundException, WCF has to be told about this exception to know how to 
        // serialize it 
        [FaultContract(typeof(NotFoundException))]
        Car GetCar(int carId);
        [OperationContract]
        Car[] GetAllCars(); // The reason for using an array instead of IEnumberable or List is because the client side,
                            // which retrieves this data is not 100% .NET, and this makes sure that non-.NET client can still 
                            // retrieve the data as well


        // Because this operation relative to I/O so it needs to have transaction, it will allow to add other things in data access
        // layer without touching the service

        // This transaction option allows the trasaction to flow into operation if it start because the service call was initiated
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteCar(int carId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Car UpdateCar(Car car);

        [OperationContract]
        Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate);
    }
}
