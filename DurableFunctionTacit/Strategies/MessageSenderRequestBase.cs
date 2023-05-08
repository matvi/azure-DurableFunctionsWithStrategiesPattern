namespace DurableFunctionTacit.Strategies
{
    public abstract class MessageSenderRequestBase
    {
        public string DurableInstanceId { get; set; }
    }
}