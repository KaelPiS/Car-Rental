using CarRental.Client.Proxies.Service_Proxies;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }

        public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
        {
            AggregateCatalog catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryClient).Assembly));


            // This will allow MEFLoader to be called in a way that it can receive a catalog of part of a catalog of assemblies from the caller
            // which will be WPF app or ASP.NET web

            if (catalogParts != null)
            {
                foreach (var part in catalogParts)
                    catalog.Catalogs.Add(part);
            }

            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}
