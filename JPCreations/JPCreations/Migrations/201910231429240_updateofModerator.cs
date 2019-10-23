namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateofModerator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Moderators", "ApplicationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Moderators", "ApplicationId");
            AddForeignKey("dbo.Moderators", "ApplicationId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderators", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Moderators", new[] { "ApplicationId" });
            DropColumn("dbo.Moderators", "ApplicationId");
        }
    }
}
