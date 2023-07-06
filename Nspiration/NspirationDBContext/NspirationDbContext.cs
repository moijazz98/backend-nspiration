using Nspiration.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Nspiration.NspirationDBContext
{
    public class NspirationDbContext:DbContext
    {
        public NspirationDbContext(DbContextOptions<NspirationDbContext> options) : base(options)
        {
        }
        public DbSet<Color> Color { get; set; }
        public DbSet<ColorFamily> ColorFamily { get; set; }
    }
}
