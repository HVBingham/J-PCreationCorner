namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movingpurhcasequant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PurchaseQuantiry", c => c.Int());
            DropColumn("dbo.Customers", "PurchaseAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "PurchaseAmount", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "PurchaseQuantiry");
        }
    }
}
