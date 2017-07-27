using Core.Common.Contract;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRental.Data.Data_Repository
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, CarRentalContext>
       where T : class, IIdentifiableEntity, new()
    {

    }
}
