namespace DurableFunctionTacit.Strategies
{
    public abstract class MessageSenderResponseBase
    {
        public string DurableInstanceId { get; set; }
        public bool Approved { get; set; }
        public string NewLicensePlate { get; set; }
        public string LicensePlate { get; set; }
    }
}