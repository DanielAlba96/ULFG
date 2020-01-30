namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioClavesCustom : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Blocks", new[] { "Blocking_user" });
            DropIndex("dbo.Blocks", new[] { "Blocked_user" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id" });
            DropIndex("dbo.GuildMembers", new[] { "Member_id" });
            DropIndex("dbo.Follows", new[] { "Following_user" });
            DropIndex("dbo.Follows", new[] { "Followed_user" });
            DropPrimaryKey("dbo.Blocks");
            DropPrimaryKey("dbo.GuildMembers");
            DropPrimaryKey("dbo.Follows");
            AlterColumn("dbo.Blocks", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Users", "Username", c => c.String(maxLength: 450));
            AlterColumn("dbo.GuildMembers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Follows", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Blocks", "Id");
            AddPrimaryKey("dbo.GuildMembers", "Id");
            AddPrimaryKey("dbo.Follows", "Id");
            CreateIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" }, unique: true);
            CreateIndex("dbo.Users", "Username", unique: true);
            CreateIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id" }, unique: true);
            CreateIndex("dbo.Follows", new[] { "Following_user", "Followed_user" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Follows", new[] { "Following_user", "Followed_user" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id", "Member_id" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" });
            DropPrimaryKey("dbo.Follows");
            DropPrimaryKey("dbo.GuildMembers");
            DropPrimaryKey("dbo.Blocks");
            AlterColumn("dbo.Follows", "Id", c => c.String());
            AlterColumn("dbo.GuildMembers", "Id", c => c.String());
            AlterColumn("dbo.Users", "Username", c => c.String(maxLength: 450));
            AlterColumn("dbo.Blocks", "Id", c => c.String());
            AddPrimaryKey("dbo.Follows", new[] { "Following_user", "Followed_user" });
            AddPrimaryKey("dbo.GuildMembers", new[] { "Guild_id", "Member_id" });
            AddPrimaryKey("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" });
            CreateIndex("dbo.Follows", "Followed_user");
            CreateIndex("dbo.Follows", "Following_user");
            CreateIndex("dbo.GuildMembers", "Member_id");
            CreateIndex("dbo.GuildMembers", "Guild_id");
            CreateIndex("dbo.Users", "Username", unique: true);
            CreateIndex("dbo.Blocks", "Blocked_user");
            CreateIndex("dbo.Blocks", "Blocking_user");
        }
    }
}
