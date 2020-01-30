namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revert2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Guilds", new[] { "Name" });
            AlterColumn("dbo.Guilds", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guilds", "Name", c => c.String(maxLength: 450));
            CreateIndex("dbo.Guilds", "Name", unique: true);
        }
    }
}
