using System.Threading.Tasks;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TransactionMonitor.Models;

namespace TransactionMonitor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            this.logger = logger;
        }

        [Topic("pubsub", "newTransaction")]
        [HttpPost("/collectTransaction")]
        public async Task<ActionResult> CollectTransaction(Transaction transaction)
        {
            logger.LogInformation($"Transaction received {transaction.AccountId} type - {transaction.TransactionType} amount - {transaction.Amount}");

            return new OkResult();
        }
    }
}
