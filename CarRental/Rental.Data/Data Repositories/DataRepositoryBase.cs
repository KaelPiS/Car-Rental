using CarRental.Data;
using Core.Common.Contract;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rental.Data.Data_Repositories
{
    public abstract class DataRepositoryBase<T>:DataRepositoryBase<T, CarRentalContext>
        where T:class, IIdentifiableEntity, new()
    {

    }
}
