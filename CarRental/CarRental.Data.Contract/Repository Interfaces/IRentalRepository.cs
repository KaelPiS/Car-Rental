using CarRental.Business.Entities;
using Core.Common.Contract;
using System.Collections.Generic;

namespace CarRental.Data.Contract.Repository_Interfaces
{
    public interface IRentalRepository:IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByCar(int carID);
        Rental GetCurrentRentalByCar(int carID);
        IEnumerable<Rental> GetCurrentlyRentedCar();
        IEnumerable<Rental> GetRentalHistoryByAccount(int accountID);

    }
}
