using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230731015632)]
    public class AddProjectTable_20230731015632 : Migration
    {
        public override void Down()
        {
            Delete.Table("Project");
        }

        public override void Up()
        {
            Create.Table("Project")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("ExistingToNewId").AsInt64().NotNullable()
                .WithColumn("Base64_String").AsString().NotNullable()
                .WithColumn("SVG_String").AsString().NotNullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedBy").AsInt32().Nullable()
                .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable();
        }
    }
}
