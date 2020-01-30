namespace ULFGService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bytes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ImageTmp", c => c.Binary());
            Sql("Update dbo.Users SET ImageTmp = Convert(varbinary, Image)");
            DropColumn("dbo.Users", "Image");
            RenameColumn("dbo.Users", "ImageTmp", "Image");
            AddColumn("dbo.Channels", "ImageTmp", c => c.Binary());
            Sql("Update dbo.Channels SET ImageTmp = Convert(varbinary, Image)");
            DropColumn("dbo.Channels", "Image");
            RenameColumn("dbo.Channels", "ImageTmp", "Image");
            AddColumn("dbo.Publications", "AttachmentTmp", c => c.Binary());
            Sql("Update dbo.Publications SET AttachmentTmp = Convert(varbinary, Attachment)");
            DropColumn("dbo.Publications", "Attachment");
            RenameColumn("dbo.Publications", "AttachmentTmp", "Attachment");

        }
        
        public override void Down()
        {
            AlterColumn("dbo.Publications", "Attachment", c => c.String());
            AlterColumn("dbo.Channels", "Image", c => c.String());
            AlterColumn("dbo.Users", "Image", c => c.String());
        }
    }
}
