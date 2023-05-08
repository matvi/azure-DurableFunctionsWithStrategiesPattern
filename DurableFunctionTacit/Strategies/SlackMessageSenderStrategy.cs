using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DurableFunctionTacit.Strategies
{
	public class SlackMessageSenderStrategy : IMessageSender
	{
        private readonly ILogger<SlackMessageSenderStrategy> _logger;

        public SlackMessageSenderStrategy(ILogger<SlackMessageSenderStrategy> logger)
		{
            _logger = logger;
        }

        public Task<MessageSenderResponseBase> SendMessage(MessageSenderRequestBase messageSenderRequestBase)
        {
            _logger.LogError("Sending Message to Slack API to do manual recognition");
            var response = new SlackMessageSenderResponse
            {
                Approved = true
            };

            return Task.FromResult<MessageSenderResponseBase>(response);
        }
    }
}

