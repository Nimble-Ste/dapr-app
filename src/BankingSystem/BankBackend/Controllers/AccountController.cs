using BankBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;

namespace BankBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DaprClient daprClient;

        const string storeName = "statestore";
        const string key = "accounts";

        public AccountController(DaprClient daprClient)
        {
            this.daprClient = daprClient;

            var accounts = new List<BankAccount>()
            {
                new BankAccount()
                {
                    AccountNumber = "1111-2222-3333",
                    Balance = 100.00
                },
                new BankAccount()
                {
                    AccountNumber = "2222-1111-1111",
                    Balance = 10.99
                }
            };


         //   daprClient.SaveStateAsync(storeName, key, accounts);
        }

        [HttpGet]
        public async Task<IEnumerable<BankAccount>> Get()
        {
            return await GetAccountsAsync();
        }

        public async Task<IEnumerable<BankAccount>> Post(WithdrawalRequest withdrawalRequest)
        {
            var accounts = await this.GetAccountsAsync();

            var account = accounts.First(x => x.AccountNumber == withdrawalRequest.AccountNumber);

            account.Balance -= withdrawalRequest.Amount;

            await daprClient.SaveStateAsync<List<BankAccount>>(storeName, key, accounts);

            return await this.GetAccountsAsync();
        }

        private async Task<List<BankAccount>> GetAccountsAsync()
        {
            var defaultAccounts = new List<BankAccount>()
            {
                new BankAccount()
                {
                    AccountNumber = "1111-2222-3333",
                    Balance = 100.00
                },
                new BankAccount()
                {
                    AccountNumber = "2222-1111-1111",
                    Balance = 10.99
                }
            };


            var accounts = await daprClient.GetStateAsync<List<BankAccount>>(storeName, key);

            if (accounts == null || !accounts.Any())
            {
                return defaultAccounts;
            }

            return accounts;
        }
    }
}
