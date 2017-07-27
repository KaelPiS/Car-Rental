using CarRental.Business.Entities;
using CarRental.Data.Contract.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace CarRental.Data.Data_Repository
{
    [Export(typeof(ICarRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
       

        public int myCustomMethod()
        {
            throw new NotImplementedException();
        }

        // Just normal Entity Framework
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
