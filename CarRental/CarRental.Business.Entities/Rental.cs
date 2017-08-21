using Core.Common.Contract;
using Core.Common.Cores;
using System;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Rental : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int RentalID { get; set; }

        [DataMember]
        public int AccountID { get; set; }

        [DataMember]
        public int CarID { get; set; }

        [DataMember]
        public DateTime DateRented { get; set; }

        [DataMember]
        public DateTime DateDue { get; set; }

        [DataMember]
        public DateTime? ReturnDate { get; set; }


        public int EntityID
        {
            get { return RentalID; }
            set { RentalID = value; }
        }


        int IAccountOwnedEntity.AccountOwnerID
        {
            get { return AccountID; }
        }
        
    }
}
