using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Exceptions
{
    public class CarNotRentedException:ApplicationException
    {
        public CarNotRentedException(string message)
            : this(message, null)
        {
        }

        public CarNotRentedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
