

using DurableFunctionTacit.Strategies;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(DurableFunctionTacit.Startup))]

namespace DurableFunctionTacit
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddScoped<IMessageSender,SlackMessageSenderStrategy>();

        }
    }
}
