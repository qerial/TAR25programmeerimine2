using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopTAR25.RealEstateTest.Mock
{
    public class MockIHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

    }
}
