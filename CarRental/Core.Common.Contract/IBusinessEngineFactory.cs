using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contract
{
    public interface IBusinessEngineFactory
    {
        // Doing the same work as DataRepositoryFactory
        T GetBusinessEngine<T>() where T : IBusinessEngine;
    }
}
