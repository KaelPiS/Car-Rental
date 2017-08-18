﻿using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Contract.DTOs
{
    // DTO is nothing more than a entity wrapper.
    public class CustomerRentalInfo
    {
        public Account Customer { get; set; }
        public Car Car { get; set; }
        public Rental Rental { get; set; }
    }
}