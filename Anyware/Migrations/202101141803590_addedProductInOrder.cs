namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductInOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductInOrders",
                c => new
                    {
                        ProductInOrderID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductInOrderID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductInOrders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductInOrders", "OrderID", "dbo.Orders");
            DropIndex("dbo.ProductInOrders", new[] { "OrderID" });
            DropIndex("dbo.ProductInOrders", new[] { "ProductID" });
            DropTable("dbo.ProductInOrders");
        }
    }
}
