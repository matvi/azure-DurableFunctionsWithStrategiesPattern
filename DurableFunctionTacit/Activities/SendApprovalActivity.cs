using System;
using System.Threading.Tasks;
using DurableFunctionTacit.Strategies;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctionTacit.Activities
{
	public class SendApprovalActivity
	{
        private readonly IMessageSender _messageSender;

        public SendApprovalActivity(IMessageSender messageSender)
		{
            _messageSender = messageSender;
        }

		[FunctionName(nameof(SendApprovalActivity.SendApprovalMessage))]
		public async Task<MessageSenderResponseBase> SendApprovalMessage([ActivityTrigger] SlackMessageRequest messageSenderRequestBase, ILogger logger)
		{
			var response = await _messageSender.SendMessage(messageSenderRequestBase);

			return response;
		}
	}
}

