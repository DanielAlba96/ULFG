namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keys : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Blocks");
            DropPrimaryKey("dbo.Follows");
            AlterColumn("dbo.Blocks", "Id", c => c.String());
            AlterColumn("dbo.Follows", "Id", c => c.String());
            AddPrimaryKey("dbo.Blocks", new[] { "Blocking_user", "Blocked_user" });
            AddPrimaryKey("dbo.Follows", new[] { "Following_user", "Followed_user" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Follows");
            DropPrimaryKey("dbo.Blocks");
            AlterColumn("dbo.Follows", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Blocks", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Follows", "Id");
            AddPrimaryKey("dbo.Blocks", "Id");
        }
    }
}
