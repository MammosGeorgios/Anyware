namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductCategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        VatCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryID)
                .ForeignKey("dbo.VatCategories", t => t.VatCategoryID, cascadeDelete: true)
                .Index(t => t.VatCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategories", "VatCategoryID", "dbo.VatCategories");
            DropIndex("dbo.ProductCategories", new[] { "VatCategoryID" });
            DropTable("dbo.ProductCategories");
        }
    }
}
