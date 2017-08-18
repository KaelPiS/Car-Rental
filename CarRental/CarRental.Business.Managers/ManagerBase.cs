using Core.Common.Cores;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        public ManagerBase()
        {
            // This will tell WCF to go ahead and resolve anything with the [Import] tag when it initiates the InventoryManager
            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);
        }


        // Exception Handle
        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExcecute)
        {
            try
            {
                return codeToExcecute.Invoke();
            }
            catch(FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected T ExecuteFaultHandledOperation(Action codeToExcecute)
        {
            try
            {
                codeToExcecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }


    }
}
