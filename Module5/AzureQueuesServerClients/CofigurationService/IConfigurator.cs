namespace CofigurationService
{
    public interface IConfigurator
    {
        string ServiceBusConnectionString { get; }

        string StorageAccountConnectionString { get; }

        string QueueName{ get; }

        string TopicName { get; }

        string CilentName { get; }

        string TargetDirectory { get; }

        int WaitTimeout { get; }

        int HoursAttachmentLive { get; }

        bool IsValid();
    }
}