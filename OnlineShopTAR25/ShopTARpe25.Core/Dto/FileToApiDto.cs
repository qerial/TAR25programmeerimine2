using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    public class FileToApiDto
    {
        public Guid ID { get; set; }

        //see muutuja hakkab näitama, kus asub meie file
        public string? ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
