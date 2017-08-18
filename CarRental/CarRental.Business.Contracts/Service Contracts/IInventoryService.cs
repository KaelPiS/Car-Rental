using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
    }
}
