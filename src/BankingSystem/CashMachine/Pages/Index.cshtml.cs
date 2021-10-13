using CashMachine.Models;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CashMachine.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly DaprClient _daprClient;

        public IndexModel(ILogger<IndexModel> logger, DaprClient daprClient)
        {
            _logger = logger;

            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task OnGet()
        {
            var accounts = await _daprClient.InvokeMethodAsync<IEnumerable<BankAccount>>(
              HttpMethod.Get,
              "bankbackend",
              "account");

            ViewData["Accounts"] = accounts;
        }

        public async Task OnPost(WithdrawalRequest request)
        {
            var accounts = await _daprClient.InvokeMethodAsync<WithdrawalRequest, IEnumerable<BankAccount>>(
                HttpMethod.Post,
                "bankbackend",
                "account", request);

            ViewData["Accounts"] = accounts;
        }
    }
}
