using System;

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
