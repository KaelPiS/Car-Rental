using CarRental.Business.Entities;
using CarRental.Data.Contract.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace CarRental.Data.Data_Repository
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        

        // Just normal Entity Framework
        protected override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.ReservationSet
                   select e;
        }

        protected override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.ReservationSet
                         where e.ReservationID == id
                         select e);
            return query.FirstOrDefault();
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            var query = (from e in entityContext.ReservationSet
                         where e.ReservationID == entity.ReservationID
                         select e);
            return query.FirstOrDefault();
        }
    }
}
