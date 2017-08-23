using System;
using System.Runtime.Serialization;

namespace CarRental.Client.Contracts
{
    public class CustomerReservationData
    {
        [DataMember]
        public int ReservationID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Car { get; set; }
        [DataMember]
        public DateTime RentalDate { get; set; }
        [DataMember]
        public DateTime ReturnDate { get; set; }
    }
}
