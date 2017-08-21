using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contract
{
    // Using Abstract Factory Design Pattern
    public interface IDataRepositoryFactory
    {
        T GetDataRepository<T>() where T : IDataRepository; // Get the Data Repository that implement the IDataRepository interface

        
    }
}
