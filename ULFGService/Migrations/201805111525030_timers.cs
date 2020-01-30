namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blocks", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Chats", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Messages", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Guilds", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.GuildMembers", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Follows", "Client_updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Publications", "Client_updated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publications", "Client_updated");
            DropColumn("dbo.Follows", "Client_updated");
            DropColumn("dbo.GuildMembers", "Client_updated");
            DropColumn("dbo.Guilds", "Client_updated");
            DropColumn("dbo.Messages", "Client_updated");
            DropColumn("dbo.Chats", "Client_updated");
            DropColumn("dbo.Blocks", "Client_updated");
        }
    }
}
