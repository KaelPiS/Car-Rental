using System.ComponentModel.Composition.Hosting;
using CarRental.Data.Data_Repository;
using CarRental.Business;
using CarRental.Business.Business_Engines;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRentalEngine).Assembly));
            
            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}
