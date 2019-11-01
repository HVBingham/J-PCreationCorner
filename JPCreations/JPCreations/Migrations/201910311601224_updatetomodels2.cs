namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetomodels2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        OrderAmount = c.Double(nullable: false),
                        ShippingPrice = c.Double(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.Items", "Order_OrderId", c => c.Int());
            CreateIndex("dbo.Items", "Order_OrderId");
            AddForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders", "OrderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Items", new[] { "Order_OrderId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropColumn("dbo.Items", "Order_OrderId");
            DropTable("dbo.Orders");
        }
    }
}
