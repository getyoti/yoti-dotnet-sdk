namespace Example.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddBase64Photo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Base64Photo", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Users", "Base64Photo");
        }
    }
}