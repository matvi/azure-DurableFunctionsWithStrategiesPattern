using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using DurableFunctionTacit.Models;
using DurableFunctionTacit.Functions;

namespace DurableFunctionTacit
{
    public static class DurableFunctionTacit
    {
        [FunctionName("DurableFunctionTacit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient]IDurableOrchestrationClient durableClient,
            ILogger log)
        {
            log.LogError("DurableFunctionTacit triggered by http");

            //#1 Deserialize request to SpeedVilation model
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var speedViolation = JsonConvert.DeserializeObject<SpeedViolation>(requestBody);

            //#2 Check if Recognition Acurracy is below the stablished threhold
            if(speedViolation.AccuracyRecognition < 0.5)
            {
                log.LogError("Speed Violation AcurracyRecognition below threhold, sending to Manual recognition");
                var instaceId = await durableClient.StartNewAsync(nameof(ManualOrchastrationFunction.ManualApproval), speedViolation);
                return durableClient.CreateCheckStatusResponse(req, instaceId);

            }


            log.LogError("Speed Violation AcurracyRecognition above threhold, saving record in database");
            //#3 Save the speed violation in the database
            await SaveSpeedViolationRecord(speedViolation);

            return new OkObjectResult(speedViolation);
        }

        private static Task SaveSpeedViolationRecord(SpeedViolation speedViolation)
        {
            return Task.CompletedTask;
        }
    }
}

