namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVendorSecretKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "VendorSecretKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "VendorSecretKey");
        }
    }
}
