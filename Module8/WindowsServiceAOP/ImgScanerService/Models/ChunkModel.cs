using System;
using System.Collections.Generic;

namespace ImgScanerService.Models
{
    public class ChunkModel
    {
        public IList<string> ChunkFiles { get; set; } = new List<string>();
        public string Name { get; set; }
        public DateTime Created { get; set; } = new DateTime();

    }
}
