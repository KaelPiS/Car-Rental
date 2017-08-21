using CarRental.Business.Entities;
using Core.Common.Contract;

namespace CarRental.Data.Contract.Repository_Interfaces
{
    public interface ICarRepository:IDataRepository<Car>
    {
        int myCustomMethod();
    }
}
