using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Moq;
using CarRental.Data.Contract.Repository_Interfaces;
using Core.Common.Contract;
using CarRental.Business.Business_Engines;

namespace CarRental.Business.Tests
{
    [TestClass]
    public class CarRentalEngineTests
    {
        [TestMethod]
        public void IsCarCurrentlyRented_any_account()
        {
            Rental rental = new Rental()
            {
                RentalID = 1
            };

            Mock<IRentalRepository> mockRentalRepository = new Mock<IRentalRepository>();
            mockRentalRepository.Setup(obj => obj.GetCurrentRentalByCar(1)).Returns(rental);

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<IRentalRepository>()).Returns(mockRentalRepository.Object);

            CarRentalEngine carRentalEngine = new CarRentalEngine(mockRepositoryFactory.Object);

            bool try1 = carRentalEngine.IsCarCurrentlyRented(1);
            bool try2 = carRentalEngine.IsCarCurrentlyRented(2);

            Assert.IsTrue(try1);
            Assert.IsFalse(try2);
        }
    }

}
