namespace JPCreations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtionofimages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.ImageProducts",
                c => new
                    {
                        Image_ImageId = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_ImageId, t.Product_Id })
                .ForeignKey("dbo.Images", t => t.Image_ImageId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Image_ImageId)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ImageProducts", "Image_ImageId", "dbo.Images");
            DropIndex("dbo.ImageProducts", new[] { "Product_Id" });
            DropIndex("dbo.ImageProducts", new[] { "Image_ImageId" });
            DropTable("dbo.ImageProducts");
            DropTable("dbo.Images");
        }
    }
}
