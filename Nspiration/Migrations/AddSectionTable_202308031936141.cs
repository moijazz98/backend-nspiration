using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(202308031936141)]
    public class AddSectionTable_20230803121: Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Table("Section")
               .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
               .WithColumn("FolderId").AsInt64().Nullable().ForeignKey("Folder", "Id")
               .WithColumn("ProjectId").AsInt64().Nullable().ForeignKey("Project", "Id")
               .WithColumn("PathName").AsString().NotNullable()
               .WithColumn("IsActive").AsBoolean().NotNullable()
               .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
               .WithColumn("CreatedBy").AsInt32().NotNullable()
               .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable()
               .WithColumn("ModifiedBy").AsInt32().Nullable();
        }
    }
}
