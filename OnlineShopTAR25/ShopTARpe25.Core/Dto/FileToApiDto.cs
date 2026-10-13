using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Dto
{
    public class FileToApiDto
    {
        public Guid ID { get; set; }
        public string? ExistingFilePath { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}
