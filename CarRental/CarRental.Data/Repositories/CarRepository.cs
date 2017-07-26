using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarRental.Data;
using CarRental.Data.Contract.Repository_Interfaces;
using System.ComponentModel.Composition;

namespace Rental.Data.Data_Repositories
{
    [Export(typeof(ICarRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
        public int myCustomMethod()
        {
            throw new NotImplementedException();
        }


        // Just normal LINQ
        protected override Car AddEntity(CarRentalContext entityContext, Car entity)
        {
            return entityContext.CarSet.Add(entity);
        }

        protected override IEnumerable<Car> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.CarSet
                   select e;
        }

        protected override Car GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.CarSet
                         where e.CarID == id
                         select e);
            return query.FirstOrDefault();
        }

        protected override Car UpdateEntity(CarRentalContext entityContext, Car entity)
        {
            var query = (from e in entityContext.CarSet
                         where e.CarID == entity.CarID
                         select e);
            return query.FirstOrDefault();
        }
    }
}
