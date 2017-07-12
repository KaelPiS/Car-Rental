using CarRental.Business.Entities;
using Core.Common.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CarRental.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext() : base("name=CarRental")
        {
            Database.SetInitializer<CarRentalContext>(null); //Tell the database to use this context class
        }

        public DbSet<Account> AccountSet { get; set; }

        public DbSet<Car> CarSet { get; set; }

        public DbSet<Rental> RentalSet { get; set; }
        
        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //Remove default Name Convention

            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

        }

    }
}

