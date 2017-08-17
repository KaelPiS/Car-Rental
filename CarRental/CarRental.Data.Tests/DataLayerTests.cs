using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Cores;
using CarRental.Business.Bootstrapper;
using System.ComponentModel.Composition;
using CarRental.Data.Contract.Repository_Interfaces;
using System.Collections.Generic;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contract;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();
            IEnumerable<Car> cars = repositoryTest.GetCars();
            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_factory_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();
            IEnumerable<Car> cars = repositoryTest.GetCars();
            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_mocking()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarID=1, Color="Red" },
                new Car(){CarID=2, Color="Blue"}
            };
            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj=> obj.Get()).Returns(cars); // Tell mock to return list of cars when it sees the call of Get()

            RepositoryTestClass repositoryTestClass = new RepositoryTestClass(mockCarRepository.Object);
            IEnumerable<Car> carTest = repositoryTestClass.GetCars();
            Assert.IsTrue(carTest == cars);
        }

        [TestMethod]
        public void test_repository_factory_mocking1()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarID=1, Color="Red" },
                new Car(){CarID=2, Color="Blue"}
            };
            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(cars); // Tell mock to return list of cars when it sees the call of Get()

            RepositoryFactoryTestClass repositoryTestClass = new RepositoryFactoryTestClass(mockDataRepository.Object);
            IEnumerable<Car> carTest = repositoryTestClass.GetCars();
            Assert.IsTrue(carTest == cars);
        }

        [TestMethod]
        public void test_repository_factory_mocking2()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarID=1, Color="Red" },
                new Car(){CarID=2, Color="Blue"}
            };
            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars); // Tell mock to return list of cars when it sees the call of Get()

            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object); // Tell mock to return the mock ICarRepository 
                                                                                                                // when it see the GetDataRepository<ICarRepository is called

            RepositoryFactoryTestClass repositoryTestClass = new RepositoryFactoryTestClass(mockDataRepository.Object);
            IEnumerable<Car> carTest = repositoryTestClass.GetCars();
            Assert.IsTrue(carTest == cars);
        }

    }

    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryTestClass(ICarRepository carRepository)
        {
            _CarRepository = carRepository;
        }


        [Import]
        ICarRepository _CarRepository;

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _CarRepository.Get();
            return cars;
        }
    }

    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;
        
        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }


        // This help to get some specific methods of class without wasting resouces implementing, instantiating all of the classes
        
        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

            IEnumerable<Car> cars= carRepository.Get();

            return cars;
        }
    }
}
