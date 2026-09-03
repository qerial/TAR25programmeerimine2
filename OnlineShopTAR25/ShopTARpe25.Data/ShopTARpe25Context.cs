using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Data
{
    // teha sellest classist DbContext, et saaks andmebaasi kasutada
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options) : base (options)
        {

        }
    }
}
