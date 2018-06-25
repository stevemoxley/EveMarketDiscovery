namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Long(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        Quantity = c.Long(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        TypeId = c.Long(nullable: false),
                        LocationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
        }
    }
}
