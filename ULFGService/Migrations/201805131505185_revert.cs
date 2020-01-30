namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revert : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user", "Deleted_At" });
            DropIndex("dbo.Users", new[] { "Email", "Deleted_At" });
            DropIndex("dbo.Chats", new[] { "Member1_id", "Member2_id", "Deleted_At" });
            DropIndex("dbo.Guilds", new[] { "Name", "Deleted_At" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id", "Deleted_At" });
            DropIndex("dbo.Follows", new[] { "Following_user", "Followed_user", "Deleted_At" });
            RenameIndex(table: "dbo.Users", name: "IX_Username_Deleted_At", newName: "IX_Username");
            AddColumn("dbo.Guilds", "Public", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Email", c => c.String());
            CreateIndex("dbo.Blocks", "Blocking_user");
            CreateIndex("dbo.Blocks", "Blocked_user");
            CreateIndex("dbo.Chats", "Member1_id");
            CreateIndex("dbo.Chats", "Member2_id");
            CreateIndex("dbo.Guilds", "Name", unique: true);
            CreateIndex("dbo.GuildMembers", "Guild_id");
            CreateIndex("dbo.GuildMembers", "Member_id");
            CreateIndex("dbo.Follows", "Following_user");
            CreateIndex("dbo.Follows", "Followed_user");
            DropColumn("dbo.Blocks", "Deleted_At");
            DropColumn("dbo.Users", "Deleted_At");
            DropColumn("dbo.Chats", "Deleted_At");
            DropColumn("dbo.Guilds", "Is_public");
            DropColumn("dbo.Guilds", "Deleted_At");
            DropColumn("dbo.GuildMembers", "Deleted_At");
            DropColumn("dbo.Follows", "Deleted_At");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Follows", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.GuildMembers", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Guilds", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Guilds", "Is_public", c => c.Boolean(nullable: false));
            AddColumn("dbo.Chats", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Users", "Deleted_At", c => c.DateTime());
            AddColumn("dbo.Blocks", "Deleted_At", c => c.DateTime());
            DropIndex("dbo.Follows", new[] { "Followed_user" });
            DropIndex("dbo.Follows", new[] { "Following_user" });
            DropIndex("dbo.GuildMembers", new[] { "Member_id" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id" });
            DropIndex("dbo.Guilds", new[] { "Name" });
            DropIndex("dbo.Chats", new[] { "Member2_id" });
            DropIndex("dbo.Chats", new[] { "Member1_id" });
            DropIndex("dbo.Blocks", new[] { "Blocked_user" });
            DropIndex("dbo.Blocks", new[] { "Blocking_user" });
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 450));
            DropColumn("dbo.Guilds", "Public");
            RenameIndex(table: "dbo.Users", name: "IX_Username", newName: "IX_Username_Deleted_At");
            CreateIndex("dbo.Follows", new[] { "Following_user", "Followed_user", "Deleted_At" }, unique: true);
            CreateIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Guilds", new[] { "Name", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Chats", new[] { "Member1_id", "Member2_id", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Users", new[] { "Email", "Deleted_At" }, unique: true);
            CreateIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user", "Deleted_At" }, unique: true);
        }
    }
}
