namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guilds", "Is_public", c => c.Boolean(nullable: false));
            DropColumn("dbo.Guilds", "Public");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guilds", "Public", c => c.Boolean(nullable: false));
            DropColumn("dbo.Guilds", "Is_public");
        }
    }
}
