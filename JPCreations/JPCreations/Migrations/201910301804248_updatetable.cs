namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Items", new[] { "Customer_Id" });
            DropColumn("dbo.Items", "Customer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Items", "Customer_Id");
            AddForeignKey("dbo.Items", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
