using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using CarRental.Business.Contracts;

namespace CarRental.ServiceHosts.Tests
{
    [TestClass]
    public class ServiceAcessTests
    {
        [TestMethod]
        public void test_inventory_manager_as_service()
        {
            // Chanel Factory allows to create a proxy without having an actual wrapper class known as proxy class

            // The default constructor tells the proxy to go out to the configue file and look for endpoint that exposes or that tries
            // to access this contract. If it find only one, use that one. Because the endpoint have many endpoints so we need to provide 
            // a blank in the non-default constructor
            ChannelFactory<IInventoryService> channelFactory = new ChannelFactory<IInventoryService>("");

            IInventoryService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_rental_manager_as_service()
        {
            ChannelFactory<IRentalService> channelFactory = new ChannelFactory<IRentalService>("");

            IRentalService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_account_manager_as_service()
        {
            ChannelFactory<IAccountService> channelFactory = new ChannelFactory<IAccountService>("");

            IAccountService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }
    }
}
