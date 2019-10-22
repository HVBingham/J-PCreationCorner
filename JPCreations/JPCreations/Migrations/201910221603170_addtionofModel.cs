namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtionofModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(maxLength: 2),
                        ZipCode = c.String(maxLength: 5),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Customers", new[] { "ApplicationId" });
            DropTable("dbo.Customers");
        }
    }
}
