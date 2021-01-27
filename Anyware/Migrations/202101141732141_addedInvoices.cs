namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedInvoices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        InvoiceStatus = c.Int(nullable: false),
                        PaymentDueDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "OrderID", "dbo.Orders");
            DropIndex("dbo.Invoices", new[] { "OrderID" });
            DropTable("dbo.Invoices");
        }
    }
}
