using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230822000123)]
    public class SeedingShadeCardTable_20230822000123 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Insert.IntoTable("ShadeCard").Row(new { Id = "1", Name = ("COLOUR CREATIONS PALETTE II") });
        }
    }
}
