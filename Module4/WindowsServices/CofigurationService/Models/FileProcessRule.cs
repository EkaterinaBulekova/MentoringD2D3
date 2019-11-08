using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CofigurationService
{
    public class FileProcessRule
    {
        private const int InitialIndex = -1;

        public string Prefix { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Delimeter { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumberLenght { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Extentions { get; set; }
        
        [Required]
        [Range(1000, 600000)]
        public int Timeout { get; set; }


        public List<FileInfo> GetAllMatchRule(IEnumerable<FileInfo> files)
        {
            return files.Where(f => ShouldProcess(f.Name)).OrderBy(f => GetIndex(f.Name)).ToList();
        }

        public int GetIndex(string filename)
        {
            string pattern = $"{Delimeter}{NumberPattern(NumberLenght)}";
            var match = Regex.Match(filename, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var numberStr = match.Value.Replace(Delimeter, "").Replace(".", "");
                if (int.TryParse(numberStr, out int index)) return index;
            }
            return InitialIndex;
        }        
        
        public bool ShouldProcess(string fileName)
        {
            return Regex.IsMatch(fileName, $"^{Prefix}{Delimeter}{NumberPattern(NumberLenght)}({Extentions})$");
        }

        private string NumberPattern(int lenght)
        {
            return "\\d{1," + lenght + "}\\.";
        }
    }
}
