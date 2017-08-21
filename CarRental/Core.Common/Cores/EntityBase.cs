using System.Runtime.Serialization;

namespace Core.Common.Cores
{
    [DataContract]
    public class EntityBase : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
        
    }
}
