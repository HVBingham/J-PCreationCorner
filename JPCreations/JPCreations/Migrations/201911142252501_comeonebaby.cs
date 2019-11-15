namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comeonebaby : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Items", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Items", new[] { "Product_Id" });
            DropIndex("dbo.Items", new[] { "Order_OrderId" });
            CreateTable(
                "dbo.tblOrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        OrderId = c.Int(),
                        ProductId = c.Int(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.tblOrders", t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.CustomerId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.tblOrders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.Products", "OrderDetailsDTO_Id", c => c.Int());
            CreateIndex("dbo.Products", "OrderDetailsDTO_Id");
            AddForeignKey("dbo.Products", "OrderDetailsDTO_Id", "dbo.tblOrderDetails", "Id");
            DropColumn("dbo.Products", "PurchaseQuantity");
            DropTable("dbo.Orders");
            DropTable("dbo.Items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Product_Id = c.Int(),
                        Order_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.OrderId);
            
            AddColumn("dbo.Products", "PurchaseQuantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.Products", "OrderDetailsDTO_Id", "dbo.tblOrderDetails");
            DropForeignKey("dbo.tblOrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.tblOrderDetails", "OrderId", "dbo.tblOrders");
            DropForeignKey("dbo.tblOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.tblOrderDetails", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Products", new[] { "OrderDetailsDTO_Id" });
            DropIndex("dbo.tblOrders", new[] { "CustomerId" });
            DropIndex("dbo.tblOrderDetails", new[] { "ProductId" });
            DropIndex("dbo.tblOrderDetails", new[] { "OrderId" });
            DropIndex("dbo.tblOrderDetails", new[] { "CustomerId" });
            DropColumn("dbo.Products", "OrderDetailsDTO_Id");
            DropTable("dbo.tblOrders");
            DropTable("dbo.tblOrderDetails");
            CreateIndex("dbo.Items", "Order_OrderId");
            CreateIndex("dbo.Items", "Product_Id");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders", "OrderId");
            AddForeignKey("dbo.Items", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}
