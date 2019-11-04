namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtionofItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PreviousLogIn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PreviousLogIn");
        }
    }
}
