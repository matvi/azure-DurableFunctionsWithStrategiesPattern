using System;
using System.Threading;
using System.Threading.Tasks;
using DurableFunctionTacit.Activities;
using DurableFunctionTacit.Models;
using DurableFunctionTacit.Strategies;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctionTacit.Functions
{
	public class ManualOrchastrationFunction
	{
		[FunctionName(nameof(ManualApproval))]
		public async Task<bool> ManualApproval(
			[OrchestrationTrigger] IDurableOrchestrationContext context, ILogger logger)
		{
			logger.LogError("Entering Manual Approval");
			var speedViolation = context.GetInput<SpeedViolation>();

            logger.LogError("Creating cancelation token");
            var cancelationToken = new CancellationTokenSource();
			var expirationTime = context.CurrentUtcDateTime.AddSeconds(10);
			var timerTask = context.CreateTimer(expirationTime, cancelationToken.Token);

            //### Here we make the api call to the system that will manage the approval by the user.

            var messageSenderRequest = new SlackMessageRequest
            {
                DurableInstanceId = context.InstanceId
            };

            await context.CallActivityAsync(nameof(SendApprovalActivity.SendApprovalMessage), messageSenderRequest);

			//creating task that will listen for the event to be triggered
			var messageResponseTask = context.WaitForExternalEvent<SlackMessageSenderResponse>("externalEvent");

            //wait for any task to finish first
            logger.LogError("Wait for user to respond or timer to finish");
            var winer = await Task.WhenAny(messageResponseTask, timerTask);

            if (winer == timerTask && timerTask.IsCompleted)
            {
                logger.LogError("User didn't repond, save in database as unable to recognize");
                return false;
            }

            logger.LogError("User completed the process");
            var userRespons = messageResponseTask.Result;
            if (userRespons.Approved)
            {
                logger.LogError("User approved the license recognition");
            }
            else if (!string.IsNullOrEmpty(userRespons.NewLicensePlate))
            {
                logger.LogError("User provided a new license plate");
            }

           


            //### ends approval user 

			return true;
		}
    }
}

