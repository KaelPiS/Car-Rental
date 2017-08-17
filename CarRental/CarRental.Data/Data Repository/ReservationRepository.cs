using CarRental.Business.Entities;
using CarRental.Data.Contract.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using CarRental.Data.Contract.DTOs;

namespace CarRental.Data.Data_Repository
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        public IEnumerable<CustomerReservationInfo> GetCustomerReservationInfo()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            join a in entityContext.AccountSet on r.AccountID equals a.AccountID
                            join c in entityContext.CarSet on r.CarID equals c.CarID
                            select new CustomerReservationInfo()
                            {
                                Reservation = r,
                                Customer = a,
                                Car = c
                            };
                return query.ToList().ToArray();
            }
        }

        public IEnumerable<Reservation> GetReservationByPickupDate(DateTime PickUpDate)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from e in entityContext.ReservationSet
                            where e.RentalDate < PickUpDate
                            select e;
                return query.ToArray().ToList();
            }
        }


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
