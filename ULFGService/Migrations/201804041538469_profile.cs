namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Bio", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Image");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Bio");
        }
    }
}
