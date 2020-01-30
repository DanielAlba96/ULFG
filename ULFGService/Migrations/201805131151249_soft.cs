namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soft : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Chats", new[] { "Member1_id" });
            DropIndex("dbo.Chats", new[] { "Member2_id" });
            DropIndex("dbo.Guilds", new[] { "Name" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id" });
            DropIndex("dbo.Follows", new[] { "Following_user", "Followed_user" });
            RenameIndex(table: "dbo.Users", name: "IX_Username", newName: "IX_Username_Deleted_At");
            AddColumn("dbo.Blocks", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Users", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Chats", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Guilds", "Public", c => c.Boolean(nullable: false));
            AddColumn("dbo.Guilds", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.GuildMembers", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Follows", "Deleted_At", c => c.DateTime());
            CreateIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Users", new[] { "Email", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Chats", new[] { "Member1_id", "Member2_id", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Guilds", new[] { "Name", "Deleted_At" }, unique: true);
            CreateIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Follows", new[] { "Following_user", "Followed_user", "Deleted_At" }, unique: true);
            DropColumn("dbo.Guilds", "Visibility");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guilds", "Visibility", c => c.Boolean(nullable: false));
            DropIndex("dbo.Follows", new[] { "Following_user", "Followed_user", "Deleted_At" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id", "Deleted_At" });
            DropIndex("dbo.Guilds", new[] { "Name", "Deleted_At" });
            DropIndex("dbo.Chats", new[] { "Member1_id", "Member2_id", "Deleted_At" });
            DropIndex("dbo.Users", new[] { "Email", "Deleted_At" });
            DropIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user", "Deleted_At" });
            DropColumn("dbo.Follows", "Deleted_At");
            DropColumn("dbo.GuildMembers", "Deleted_At");
            DropColumn("dbo.Guilds", "Deleted_At");
            DropColumn("dbo.Guilds", "Public");
            DropColumn("dbo.Chats", "Deleted_At");
            DropColumn("dbo.Users", "Deleted_At");
            DropColumn("dbo.Blocks", "Deleted_At");
            RenameIndex(table: "dbo.Users", name: "IX_Username_Deleted_At", newName: "IX_Username");
            CreateIndex("dbo.Follows", new[] { "Following_user", "Followed_user" }, unique: true);
            CreateIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id" }, unique: true);
            CreateIndex("dbo.Guilds", "Name", unique: true);
            CreateIndex("dbo.Chats", "Member2_id");
            CreateIndex("dbo.Chats", "Member1_id");
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" }, unique: true);
        }
    }
}
