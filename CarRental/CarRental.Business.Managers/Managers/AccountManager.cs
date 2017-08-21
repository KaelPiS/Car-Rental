using CarRental.Business.Contracts.Service_Contracts;
using Core.Common.Contract;
using System.ComponentModel.Composition;
using System.ServiceModel;
using CarRental.Business.Entities;
using CarRental.Data.Contract.Repository_Interfaces;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode =ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete =false)] 
    public class AccountManager:ManagerBase, IAccountService
    {
        public AccountManager()
        {

        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;
        public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        protected override Account LoadAuthorizationValidationAccount(string LoginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

            Account authAcc = accountRepository.GetByLogin(LoginName);

            if (authAcc == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find this account for login name {0}", LoginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            return authAcc;
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);

                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Account with login {0} is not in the database", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);
                return account;
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

                ValidateAuthorization(account);

                Account updatedAccount = accountRepository.Update(account);
            });
        }
    }
}
