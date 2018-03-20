namespace Example.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class IsAgeVerifiedConversion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "IsAgeVerified", c => c.Boolean());
        }

        public override void Down()
        {
            AlterColumn("dbo.Users", "IsAgeVerified", c => c.String());
        }
    }
}