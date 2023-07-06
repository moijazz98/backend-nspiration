using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230703893485)]
    public class AddColorFamilyTable_20230703893485 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("ColorFamily")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable();
        }
    }
}
