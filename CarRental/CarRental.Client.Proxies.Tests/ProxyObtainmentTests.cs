using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Cores;
using CarRental.Client.Bootstrapper;
using CarRental.Client.Contracts;
using CarRental.Client.Proxies.Service_Proxies;
using Core.Common.Contract;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void obtain_proxy_from_container_using_service_contract()
        {
            IInventoryService inventoryProxy = ObjectBase.Container.GetExportedValue<IInventoryService>();

            Assert.IsTrue(inventoryProxy is InventoryClient);
        }

        [TestMethod]

        public void obtain_proxy_from_service_factory()
        {
            IServiceFactory serviceFactory = new ServiceFactory();

            IInventoryService inventoryProxy = serviceFactory.CreateClient<IInventoryService>();

            Assert.IsTrue(inventoryProxy is InventoryClient);

        }

        [TestMethod]
        public void obtain_service_factory_and_proxy_from_container()
        {
            IServiceFactory factory = ObjectBase.Container.GetExportedValue<IServiceFactory>();

            IInventoryService inventoryProxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(inventoryProxy is InventoryClient);
        }
    }
}
