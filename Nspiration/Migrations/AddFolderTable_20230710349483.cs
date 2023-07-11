using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230710349483)]
    public class AddFolderTable_20230710349483 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("Folder")
               .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
               .WithColumn("ProjectId").AsInt64().NotNullable()
               .WithColumn("Name").AsString().Nullable()
               .WithColumn("IsActive").AsBoolean().NotNullable()
               .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
               .WithColumn("CreatedBy").AsInt32().NotNullable()
               .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable()
               .WithColumn("ModifiedBy").AsInt32().Nullable();
        }
    }
}
