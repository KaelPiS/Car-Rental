using System.Collections.Generic;
using System.Linq;
using CarRental.Data;
using CarRental.Data.Contract.Repository_Interfaces;
using System.ComponentModel.Composition;

namespace Rental.Data.Data_Repositories
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class RentalRepository : DataRepositoryBase<CarRental.Business.Entities.Rental>, IRentalRepository
    {
        

        // Just normal LINQ
        protected override CarRental.Business.Entities.Rental AddEntity(CarRentalContext entityContext, CarRental.Business.Entities.Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override IEnumerable<CarRental.Business.Entities.Rental> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.RentalSet
                   select e;
        }

        protected override CarRental.Business.Entities.Rental GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.RentalSet
                         where e.RentalID == id
                         select e);
            return query.FirstOrDefault();
        }

        protected override CarRental.Business.Entities.Rental UpdateEntity(CarRentalContext entityContext, CarRental.Business.Entities.Rental entity)
        {
            var query = (from e in entityContext.RentalSet
                         where e.RentalID == entity.RentalID
                         select e);
            return query.FirstOrDefault();
        }
    }
}
