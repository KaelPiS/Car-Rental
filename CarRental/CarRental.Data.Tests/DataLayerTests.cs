using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Cores;
using CarRental.Business.Bootstrapper;
using System.ComponentModel.Composition;
using CarRental.Data.Contract.Repository_Interfaces;
using System.Collections.Generic;
using CarRental.Business.Entities;

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
}
