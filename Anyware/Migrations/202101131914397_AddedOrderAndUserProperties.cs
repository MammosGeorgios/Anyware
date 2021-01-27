namespace Anyware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderAndUserProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DateStarted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "MiddleName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateofBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "PersonalPhone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "PersonalPhone");
            DropColumn("dbo.AspNetUsers", "DateofBirth");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "MiddleName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.Orders");
        }
    }
}
