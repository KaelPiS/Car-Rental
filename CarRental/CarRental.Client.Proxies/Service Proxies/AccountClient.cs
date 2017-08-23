using CarRental.Client.Contracts;
using System.ServiceModel;
using System.Threading.Tasks;
using CarRental.Client.Entities;
using System.ComponentModel.Composition;
using Core.Common.ServiceModel;

namespace CarRental.Client.Proxies.Service_Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountClient : UserClientBase<IAccountService>, IAccountService
    {
        public Account GetCustomerAccountInfo(string loginEmail) => Channel.GetCustomerAccountInfo(loginEmail);

        public Task<Account> GetCustomerAccountInfoAsync(string loginEmail) => Channel.GetCustomerAccountInfoAsync(loginEmail);

        public void UpdateCustomerAccountInfo(Account account) => Channel.UpdateCustomerAccountInfo(account);

        public Task UpdateCustomerAccountInfoAsync(Account account) => Channel.UpdateCustomerAccountInfoAsync(account);

    }
}
