namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Product_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Items", "Product_Id", "dbo.Products");
            DropIndex("dbo.Items", new[] { "Customer_Id" });
            DropIndex("dbo.Items", new[] { "Product_Id" });
            DropTable("dbo.Items");
        }
    }
}
