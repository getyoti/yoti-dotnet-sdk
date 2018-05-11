namespace Example.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YotiAttribute : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "FullName");
            DropColumn("dbo.Users", "GivenNames");
            DropColumn("dbo.Users", "FamilyName");
            DropColumn("dbo.Users", "MobileNumber");
            DropColumn("dbo.Users", "EmailAddress");
            DropColumn("dbo.Users", "DateOfBirth");
            DropColumn("dbo.Users", "IsAgeVerified");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "Nationality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Nationality", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "IsAgeVerified", c => c.Boolean());
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.Users", "EmailAddress", c => c.String());
            AddColumn("dbo.Users", "MobileNumber", c => c.String());
            AddColumn("dbo.Users", "FamilyName", c => c.String());
            AddColumn("dbo.Users", "GivenNames", c => c.String());
            AddColumn("dbo.Users", "FullName", c => c.String());
        }
    }
}
