using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DurableFunctionTacit.Strategies;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableFunctionTacit.Functions
{
    public static class SlackResponseActivity
    {
        [FunctionName("SlackResponseActivity")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient]IDurableClient durableClient,
            ILogger log)
        {
            log.LogInformation("User Send Response ");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userResponse = JsonConvert.DeserializeObject<SlackMessageSenderResponse>(requestBody);

            if(userResponse is null)
            {
                return new BadRequestObjectResult("Error on User response");
            }

            await durableClient.RaiseEventAsync(userResponse.DurableInstanceId, "externalEvent", userResponse);

            return new OkObjectResult(userResponse);
        }
    }
}

