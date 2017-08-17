using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Contract.DTOs
{
    public class CustomerReservationInfo
    {
        public Account Customer { get; set; }
        public Reservation Reservation { get; set; }
        public Car Car { get; set; }
    }
}
