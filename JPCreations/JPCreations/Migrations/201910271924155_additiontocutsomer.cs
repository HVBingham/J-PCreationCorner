namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additiontocutsomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "PurchaseAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "PurchaseAmount");
        }
    }
}
