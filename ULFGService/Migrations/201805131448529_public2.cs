namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class public2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guilds", "Is_public", c => c.Boolean(nullable: false));
            DropColumn("dbo.Guilds", "IsPublic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guilds", "IsPublic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Guilds", "Is_public");
        }
    }
}
