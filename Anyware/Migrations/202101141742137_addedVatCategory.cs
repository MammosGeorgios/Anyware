namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVatCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VatCategories",
                c => new
                    {
                        VatCategoryID = c.Int(nullable: false, identity: true),
                        VatName = c.String(),
                        VatPercentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.VatCategoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VatCategories");
        }
    }
}
