using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UnoCash.Core;

namespace UnoCash.Api
{
    public static class GetExpenses
    {
        static Expense[] GetAllExpenses(string account) => new[]
        {
            new Expense
            {
                Id = 1,
                Amount = 10.99m,
                Account = account,
                Date = DateTime.Today.AddDays(-1),
                Description = "Pizza",
                Status = "Reconciled",
                Type = "Regular"
            },
            new Expense
            {
                Id = 2,
                Amount = 23.90m,
                Account = account,
                Date = DateTime.Today.AddDays(-2),
                Description = "Thai",
                Status = "Reconciled",
                Type = "Regular"
            },
            new Expense
            {
                Id = 3,
                Amount = 9.80m,
                Account = account,
                Date = DateTime.Today.AddDays(-3),
                Description = "Burger",
                Status = "Pending",
                Type = "Regular"
            }
        };

        [FunctionName("GetExpenses")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var account = req.Query["account"];

            log.LogWarning($"Fetching expense(s) for account: {account}");

            return new OkObjectResult(int.TryParse(req.Query["id"], out var id) ?
                                      GetAllExpenses(account).Where(e => e.Id == id) :
                                      GetAllExpenses(account));
        }
    }
}