namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removalofunneeded : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "EmailAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "EmailAddress", c => c.String());
        }
    }
}
