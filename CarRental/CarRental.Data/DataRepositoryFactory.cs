using Core.Common.Contract;
using Core.Common.Cores;
using System.ComponentModel.Composition;

namespace CarRental.Data.Contract
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataRepositoryFacetory : IDataRepositoryFactory
    {
        public T GetDataRepository<T>() where T : IDataRepository
        {
            // When something calls upon the GetDataRepository<T>, if T was IAccountRepository (for example), it will go to the Container
            // and return class that associated with IAccountRepository (which is AccountRepository class)
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}