using FluentMigrator;

namespace Nspiration.Migrations
{
   //[Migration(202307044833981)]
    public class AddColorTable_20230704483398 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("Color")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("FamilyId").AsInt32().NotNullable().ForeignKey("ColorFamily","Id")
                .WithColumn("ShadeName").AsString().NotNullable()
                .WithColumn("ShadeCode").AsString().NotNullable()
                .WithColumn("R").AsInt32().NotNullable()
                .WithColumn("G").AsInt32().NotNullable()
                .WithColumn("B").AsInt32().NotNullable()
                .WithColumn("HexCode").AsString().NotNullable()
                .WithColumn("ShadeCardId").AsInt32().Nullable().ForeignKey("ShadeCard", "Id");
        }
    }
}
