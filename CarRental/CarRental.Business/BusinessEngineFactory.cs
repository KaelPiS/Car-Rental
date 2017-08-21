using Core.Common.Contract;
using Core.Common.Cores;

namespace CarRental.Business
{
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        // Just go to the Container and get the exported value
        public T GetBusinessEngine<T>() where T : IBusinessEngine
        {
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}
