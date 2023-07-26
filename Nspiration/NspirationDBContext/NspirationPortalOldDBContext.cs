using FluentMigrator.Runner.Processors.DB2.iSeries;
using Microsoft.EntityFrameworkCore;
using Nspiration.Model;

namespace Nspiration.NspirationDBContext
{
    public class NspirationPortalOldDBContext:DbContext
    {
        public NspirationPortalOldDBContext(DbContextOptions<NspirationPortalOldDBContext> options) : base(options)
        {
        }
        public DbSet<tblProjectTx> tblProjectTx { get; set; }
        public DbSet<tblUserM> tblUserM { get; set; }
        public DbSet<tblDepotM> tblDepotM { get; set; }
    }
}
