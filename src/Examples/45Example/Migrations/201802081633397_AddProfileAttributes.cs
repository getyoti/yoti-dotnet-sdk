namespace Example.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddProfileAttributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FullName", c => c.String());
            AddColumn("dbo.Users", "GivenNames", c => c.String());
            AddColumn("dbo.Users", "FamilyName", c => c.String());
            AddColumn("dbo.Users", "MobileNumber", c => c.String());
            AddColumn("dbo.Users", "EmailAddress", c => c.String());
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.Users", "IsAgeVerified", c => c.String());
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "Nationality", c => c.String());
            DropColumn("dbo.Users", "PhoneNumber");
        }

        public override void Down()
        {
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            DropColumn("dbo.Users", "Nationality");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "IsAgeVerified");
            DropColumn("dbo.Users", "DateOfBirth");
            DropColumn("dbo.Users", "EmailAddress");
            DropColumn("dbo.Users", "MobileNumber");
            DropColumn("dbo.Users", "FamilyName");
            DropColumn("dbo.Users", "GivenNames");
            DropColumn("dbo.Users", "FullName");
        }
    }
}