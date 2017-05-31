using Core.Common.Contract;
using Core.Common.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Reservation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int ReservationID { get; set; }

        [DataMember]
        public int AccountID { get; set; }

        [DataMember]
        public int CarID { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }

 

        public int EntityID
        {
            get { return ReservationID; }
            set { ReservationID = value; }
        }



        int IAccountOwnedEntity.AccountOwnerID
        {
            get { return AccountID; }
        }

    }
}
