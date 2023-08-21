using FluentMigrator;

namespace Nspiration.Migrations
{
  //  [Migration(20230708238298)]
    public class AddSectionColorTable_20230708238298 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("SectionColor")
              .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
              .WithColumn("SectionId").AsInt64().Nullable().ForeignKey("Section", "Id")
              .WithColumn("TypeId").AsInt32().Nullable().ForeignKey("ImageType", "Id")
              .WithColumn("ColorId").AsInt32().Nullable().ForeignKey("Color","Id") 
              .WithColumn("IsActive").AsBoolean().NotNullable()
              .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
              .WithColumn("CreatedBy").AsInt32().NotNullable()
              .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable()
              .WithColumn("ModifiedBy").AsInt32().Nullable();
        }
    }
}
