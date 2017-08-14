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
    public class Car:EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int CarID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public decimal RentalPrice { get; set; }
        public int EntityID
        {
            get
            {
                return EntityID;
            }
            set
            {
                EntityID = value;
            }
        }




        [DataMember]
        public bool CurrentlyRented { get; set; }
    }
}
