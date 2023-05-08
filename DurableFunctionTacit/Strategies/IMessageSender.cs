using System;
using System.Threading.Tasks;

namespace DurableFunctionTacit.Strategies
{
	public interface IMessageSender
	{
		public Task<MessageSenderResponseBase> SendMessage(MessageSenderRequestBase messageSenderRequestBase);
	}
}

