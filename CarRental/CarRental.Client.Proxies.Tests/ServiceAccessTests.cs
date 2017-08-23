using CarRental.Client.Proxies.Service_Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            InventoryClient proxy = new InventoryClient();
            // This proxy does not need to be casted in to ICommunicationObject in order to Open, because ClientBase class have already implemented this method
            proxy.Open();
        }

        [TestMethod]
        public void test_rental_client_connection()
        {
            RentalClient proxy = new RentalClient();
            
            proxy.Open();
        }

        [TestMethod]
        public void test_account_client_connection()
        {
            AccountClient proxy = new AccountClient();

            proxy.Open();
        }
    }
}
