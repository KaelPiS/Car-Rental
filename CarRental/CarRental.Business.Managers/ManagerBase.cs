using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Common.Exceptions;
using Core.Common.Contract;
using Core.Common.Cores;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        public ManagerBase()
        {
            // IncomingMessageHeaders is a variable that we have available from every service call
            // OperationContext is variable that available without instantiate
            OperationContext context = OperationContext.Current;
            if (context!=null)
            {
                _LoginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                // This checks that the request is actual come from the Desktop application ( with the "\" )
                if (_LoginName.IndexOf(@"\") > 1)
                    _LoginName = string.Empty;

                if (!string.IsNullOrWhiteSpace(_LoginName))
                    _AuthorizationAccount = LoadAuthorizationValidationAccount(_LoginName);
            }
            // This will tell WCF to go ahead and resolve anything with the [Import] tag when it initiates the InventoryManager
            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);
        }
        // The actual user will be implemented in the actual service inself, not in the base class. 
        // If user authorization is not needed, this method does not need to be overrided.
        protected virtual Account LoadAuthorizationValidationAccount(string LoginName)
        {
            return null;
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_AuthorizationAccount != null)
                {
                    if (_LoginName != string.Empty && entity.AccountOwnerID != _AuthorizationAccount.AccountID)
                    {
                        AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record for another user");
                        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);

                    }
                }
            }
        }

        protected Account _AuthorizationAccount = null;
        protected string _LoginName;

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

        protected void ExecuteFaultHandledOperation(Action codeToExcecute)
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
