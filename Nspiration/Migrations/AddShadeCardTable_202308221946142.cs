using FluentMigrator;

namespace Nspiration.Migrations
{
    //[Migration(202306221946142)]
    public class AddShadeCardTable_202308221946142 :Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Table("ShadeCard")
               .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
               .WithColumn("Name").AsString().NotNullable();                
        }
    }
}
