using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Exceptions
{
    public class CarCurrentlyRentedException:ApplicationException
    {
        public CarCurrentlyRentedException(string message)
            : this(message, null)
        {
        }

        public CarCurrentlyRentedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
