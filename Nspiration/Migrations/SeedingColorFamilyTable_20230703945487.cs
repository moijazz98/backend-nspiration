using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230703945487)]
    public class SeedingColorFamilyTable_20230703945487 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Insert.IntoTable("ColorFamily").Row(new { Id = "1", Name = ("Off White") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "2", Name = ("Accents") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "3", Name = ("Blue & Green") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "4", Name = ("Neutrals") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "5", Name = ("Purple and Blue") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "6", Name = ("Red") });
            Insert.IntoTable("ColorFamily").Row(new { Id = "7", Name = ("Yellow & Oranges") });
        }
    }
}
