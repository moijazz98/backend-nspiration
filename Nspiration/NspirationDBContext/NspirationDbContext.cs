using Nspiration.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FluentMigrator.Runner.Processors.DB2.iSeries;

namespace Nspiration.NspirationDBContext
{
    public class NspirationDbContext:DbContext
    {
        public NspirationDbContext(DbContextOptions<NspirationDbContext> options) : base(options)
        {
        }
        public DbSet<Color> Color { get; set; }
        public DbSet<ColorFamily> ColorFamily { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<ProjectExistingToNew> ProjectExistingToNew { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ImageInstance> ImageInstance { get; set; }
        
        public DbSet<ImageType> ImageType { get; set; }
        public DbSet<Section> Section { get; set; }
    }
}
