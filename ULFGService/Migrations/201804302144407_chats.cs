namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chats : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Chats", new[] { "Member1_id" });
            DropIndex("dbo.Chats", new[] { "Member2_id" });
            AlterColumn("dbo.Chats", "Member1_id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Chats", "Member2_id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Chats", "Member1_id");
            CreateIndex("dbo.Chats", "Member2_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Chats", new[] { "Member2_id" });
            DropIndex("dbo.Chats", new[] { "Member1_id" });
            AlterColumn("dbo.Chats", "Member2_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Chats", "Member1_id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Chats", "Member2_id");
            CreateIndex("dbo.Chats", "Member1_id");
        }
    }
}
