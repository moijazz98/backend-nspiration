using FluentMigrator;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nspiration.Migrations
{
    [Migration(20230731001235)]
    public class AddExistingProjectTable_20230731001235 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("ExistingProject")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProjectRequestId").AsInt32().NotNullable()
                .WithColumn("VendorId").AsInt32().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
                .WithColumn("ModifiedBy").AsInt32().Nullable()
                .WithColumn("ModifiedAt").AsDateTimeOffset().Nullable();
        }
    }
}
