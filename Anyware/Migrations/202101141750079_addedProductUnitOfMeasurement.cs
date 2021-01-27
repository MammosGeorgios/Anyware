namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductUnitOfMeasurement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductUnitOfMeasurements",
                c => new
                    {
                        ProductUnitOfMeasurementID = c.Int(nullable: false, identity: true),
                        UnitName = c.String(),
                        UnitAbbreviation = c.String(),
                    })
                .PrimaryKey(t => t.ProductUnitOfMeasurementID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductUnitOfMeasurements");
        }
    }
}
