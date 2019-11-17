using System.ComponentModel.DataAnnotations;

namespace CofigurationService
{
    public class MoverSettings
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string TargetDirectory { get; set; }

        [Required]
        public int WaitTimeout { get; set; }

        [Required]
        public int HoursAttachmentLive { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string ServiceBusConnectionString { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string StorageAccountConnectionString { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string QueueName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string TopicName { get; set; }

        public string ClientName { get; set; }
    }
}
