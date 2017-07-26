using CarRental.Business.Entities;
using Core.Common.Contract;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Serialization;

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

        public DbSet<CarRental.Business.Entities.Rental> RentalSet { get; set; }

        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //Remove default Name Convention

            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            //Set the primary key for each table in the database
            //Although we have already ignored the interface IIdentifiableEntity which contains EntityID but for some reasons the Entity Framework does not ignore it.
            //So this ignore below just to make sure
            modelBuilder.Entity<Account>().HasKey<int>(e => e.AccountID).Ignore(e => e.EntityID);
            modelBuilder.Entity<Car>().HasKey<int>(e => e.CarID).Ignore(e => e.EntityID);
            modelBuilder.Entity<CarRental.Business.Entities.Rental>().HasKey<int>(e => e.RentalID).Ignore(e => e.EntityID);
            modelBuilder.Entity<Reservation>().HasKey<int>(e => e.ReservationID).Ignore(e => e.EntityID);
            modelBuilder.Entity<Car>().Ignore(e => e.CurrentlyRented);


        }

    }
}

