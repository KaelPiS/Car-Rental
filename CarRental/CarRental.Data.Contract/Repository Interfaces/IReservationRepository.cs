using CarRental.Business.Entities;
using CarRental.Data.Contract.DTOs;
using Core.Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRental.Data.Contract.Repository_Interfaces
{
    public interface IReservationRepository:IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationByPickupDate(DateTime PickUpDate);
        IEnumerable<CustomerReservationInfo> GetCustomerReservationInfo();
    }
}
