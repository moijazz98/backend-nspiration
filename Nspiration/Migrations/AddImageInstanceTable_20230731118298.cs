using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230731118298)]
    public class AddImageInstanceTable_20230731118298 : Migration
    {
        public override void Down()
        {

        }
        public override void Up()
        {
            Create.Table("ImageInstance")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProjectId").AsInt64().NotNullable().ForeignKey("Project","Id")
                .WithColumn("TypeId").AsInt32().NotNullable().ForeignKey("ImageType", "Id")
                .WithColumn("SVG_String").AsString().NotNullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedBy").AsInt32().Nullable()
                .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable();
        }
    }
}
