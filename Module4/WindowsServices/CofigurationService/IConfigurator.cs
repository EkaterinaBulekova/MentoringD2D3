namespace CofigurationService
{
    public interface IConfigurator
    {
        Directories Directories { get; }
        FileProcessRule ProcessRule { get; }

        bool IsValid();
    }
}