using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Exceptions
{
    public class UnableToRentForDateException:ApplicationException
    {
        public UnableToRentForDateException(string message)
            : this(message, null)
        {
        }

        public UnableToRentForDateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
