using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230731013288)]
    public class AddImageTypeSeeding_20230731013288 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Insert.IntoTable("ImageType").Row(new { Id = "1", Name = ("Nippon 1"), IsActive=true });
            Insert.IntoTable("ImageType").Row(new { Id = "2", Name = ("Nippon 2"), IsActive = true });
            Insert.IntoTable("ImageType").Row(new { Id = "3", Name = ("Nippon 3"), IsActive = true });
            Insert.IntoTable("ImageType").Row(new { Id = "4", Name = ("Nippon 4"), IsActive = true });
            Insert.IntoTable("ImageType").Row(new { Id = "5", Name = ("Customer"), IsActive = true });
        }
    }
}
