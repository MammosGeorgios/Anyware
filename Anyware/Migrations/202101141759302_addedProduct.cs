namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Description = c.String(),
                        ProductStatus = c.Int(nullable: false),
                        ProductCategoryID = c.Int(nullable: false),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductUnitOfMeasurementID = c.Int(nullable: false),
                        UnitsInStock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ProductUnitOfMeasurements", t => t.ProductUnitOfMeasurementID, cascadeDelete: true)
                .Index(t => t.ProductCategoryID)
                .Index(t => t.ProductUnitOfMeasurementID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductUnitOfMeasurementID", "dbo.ProductUnitOfMeasurements");
            DropForeignKey("dbo.Products", "ProductCategoryID", "dbo.ProductCategories");
            DropIndex("dbo.Products", new[] { "ProductUnitOfMeasurementID" });
            DropIndex("dbo.Products", new[] { "ProductCategoryID" });
            DropTable("dbo.Products");
        }
    }
}
