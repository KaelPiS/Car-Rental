﻿using Core.Common.Contract;
using Core.Common.Cores;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{
    [DataContract]
        public class Account : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
        {
            [DataMember]
            public int AccountID { get; set; }

            [DataMember]
            public string LoginEmail { get; set; }

            [DataMember]
            public string FirstName { get; set; }

            [DataMember]
            public string LastName { get; set; }

            [DataMember]
            public string Address { get; set; }

            [DataMember]
            public string City { get; set; }

            [DataMember]
            public string State { get; set; }

            [DataMember]
            public string ZipCode { get; set; }

            [DataMember]
            public string CreditCard { get; set; }

            [DataMember]
            public string ExpDate { get; set; }

          

            public int EntityID
            {
                get { return AccountID; }
                set { AccountID = value; }
            }

        public int AccountOwnerID { get { return AccountID; } }
    }
    }
