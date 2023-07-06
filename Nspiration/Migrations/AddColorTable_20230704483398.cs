using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230704483398)]
    public class AddColorTable_20230704483398 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("Color")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("FamilyId").AsInt32().NotNullable()
                .WithColumn("ShadeName").AsString().NotNullable()
                .WithColumn("ShadeCode").AsString().NotNullable()
                .WithColumn("FamilyId").AsInt32().NotNullable()
                .WithColumn("R").AsInt32().NotNullable()
                .WithColumn("G").AsInt32().NotNullable()
                .WithColumn("B").AsInt32().NotNullable()
                .WithColumn("HexCode").AsString().NotNullable();

        }
    }
}
