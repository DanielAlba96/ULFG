namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newChatField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "Member1_deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Chats", "Member2_deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "Member2_deleted");
            DropColumn("dbo.Chats", "Member1_deleted");
        }
    }
}
