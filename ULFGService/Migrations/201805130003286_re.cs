namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class re : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guilds", "Name", c => c.String(maxLength: 450));
            CreateIndex("dbo.Guilds", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Guilds", new[] { "Name" });
            AlterColumn("dbo.Guilds", "Name", c => c.String());
        }
    }
}
