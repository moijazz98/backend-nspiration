using FluentMigrator;

namespace Nspiration.Migrations
{
    [Migration(20230731001383)]
    public class AddImageTypeTable_20230731001383 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Table("ImageType")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();
        }
    }
}
