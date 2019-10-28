namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updationofmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PurchaseQuantity", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "PurchaseQuantiry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "PurchaseQuantiry", c => c.Int());
            DropColumn("dbo.Products", "PurchaseQuantity");
        }
    }
}
