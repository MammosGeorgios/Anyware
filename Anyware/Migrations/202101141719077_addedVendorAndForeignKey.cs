namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVendorAndForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorID = c.Int(nullable: false, identity: true),
                        VendorName = c.String(),
                        VendorAFM = c.String(),
                        VendorLegalName = c.String(),
                        VendorDOI = c.String(),
                    })
                .PrimaryKey(t => t.VendorID);
            
            AddColumn("dbo.AspNetUsers", "VendorID", c => c.Int(nullable: true));
            CreateIndex("dbo.AspNetUsers", "VendorID");
            AddForeignKey("dbo.AspNetUsers", "VendorID", "dbo.Vendors", "VendorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "VendorID", "dbo.Vendors");
            DropIndex("dbo.AspNetUsers", new[] { "VendorID" });
            DropColumn("dbo.AspNetUsers", "VendorID");
            DropTable("dbo.Vendors");
        }
    }
}
