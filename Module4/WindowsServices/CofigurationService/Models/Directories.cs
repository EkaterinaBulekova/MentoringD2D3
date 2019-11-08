using System.ComponentModel.DataAnnotations;

namespace CofigurationService
{
    public class Directories
    {
        [Required]
        public string[] WatchedDirectories { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string TargetDirectory { get; set; }

        [Required]
        public int WatchTimeout { get; set; }
    }
}
