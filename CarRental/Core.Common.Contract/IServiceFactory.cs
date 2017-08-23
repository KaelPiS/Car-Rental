using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contract
{
    public interface IServiceFactory
    {
        T CreateClient<T>() where T : IServiceContract;
    }
}
