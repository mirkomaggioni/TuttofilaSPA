namespace TuttofilaSPA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreazioneTabelle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Servizi",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(),
                        SalaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sale", t => t.SalaId, cascadeDelete: true)
                .Index(t => t.SalaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizi", "SalaId", "dbo.Sale");
            DropIndex("dbo.Servizi", new[] { "SalaId" });
            DropTable("dbo.Servizi");
            DropTable("dbo.Sale");
        }
    }
}
