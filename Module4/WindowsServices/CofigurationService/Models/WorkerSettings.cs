using System.ComponentModel.DataAnnotations;

namespace CofigurationService
{
    public class WorkerSettings
    {
        [Required]
        public Directories Directories { get; set; }

        [Required]
        public FileProcessRule FileProcessRule { get; set; }
    }
}
