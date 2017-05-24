using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Cores
{
    [DataContract]
    public class EntitesBase : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
      
    }
}
