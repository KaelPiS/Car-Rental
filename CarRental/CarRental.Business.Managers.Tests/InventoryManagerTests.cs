using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Core.Common.Contract;
using Moq;
using CarRental.Data.Contract.Repository_Interfaces;
using CarRental.Business.Managers.Managers;
using System.Security.Principal;
using System.Threading;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        // Init a permission role for unit test in order to run the test
        public void Initialize()
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Khang"), new string[] {"CarRentalAdmin"});
            Thread.CurrentPrincipal = principal;
        }
        [TestMethod]
        public void UpdateCar_add_new()
        {
            Car newCar = new Car();
            Car addedCar = new Car() { CarID = 1 };

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            InventoryManager inventoryManager = new InventoryManager(mockRepositoryFactory.Object);

            Car result = inventoryManager.UpdateCar(newCar);

            Assert.IsTrue(result == addedCar);

        }


        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car existingCar = new Car() { CarID = 1 };
            Car updatedCar = new Car() { CarID = 1 };

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            InventoryManager inventoryManager = new InventoryManager(mockRepositoryFactory.Object);

            Car result = inventoryManager.UpdateCar(existingCar);

            Assert.IsTrue(result == updatedCar);
        }
    }
}
