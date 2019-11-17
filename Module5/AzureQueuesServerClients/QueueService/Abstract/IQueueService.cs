using System.Threading;
using System.Threading.Tasks;

namespace FileQueueService
{
    public interface IQueueService
    {
        Task Start(CancellationToken token);

        Task Status(ServiceStatus status);

        Task Shutdown();

        Task CleanUp();
    }
}