using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Common.ServiceModel
{
    public abstract class UserClientBase<T> : ClientBase<T>
        where T : class
    {
        public UserClientBase()
        {
            
            string userName = Thread.CurrentPrincipal.Identity.Name;
            MessageHeader<string> messageHeader = new MessageHeader<string>(userName);

            // OperationContextScope allow to callback or modify the message or header
            OperationContextScope contextScope = new OperationContextScope(InnerChannel);

            OperationContext.Current.OutgoingMessageHeaders.Add(messageHeader.GetUntypedHeader("String", "System"));
        }
    }
}
