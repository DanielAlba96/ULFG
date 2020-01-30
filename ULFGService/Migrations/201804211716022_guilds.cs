namespace ULFGService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class guilds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Channels", "Creator_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Channel_id", "dbo.Channels");
            DropForeignKey("dbo.ChannelMembers", "Channel_id", "dbo.Channels");
            DropForeignKey("dbo.ChannelMembers", "Member_id", "dbo.Users");
            DropIndex("dbo.ChannelMembers", new[] { "Channel_id" });
            DropIndex("dbo.ChannelMembers", new[] { "Member_id" });
            DropIndex("dbo.ChannelMembers", new[] { "CreatedAt" });
            DropIndex("dbo.Channels", new[] { "Creator_id" });
            DropIndex("dbo.Channels", new[] { "CreatedAt" });
            DropIndex("dbo.Messages", new[] { "Channel_id" });
            CreateTable(
                "dbo.Guilds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        Name = c.String(),
                        Image = c.Binary(),
                        Description = c.String(),
                        Visibility = c.Boolean(nullable: false),
                        Leader_id = c.String(nullable: false, maxLength: 128),
                        Guild_message = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Leader_id)
                .Index(t => t.Leader_id)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "dbo.GuildMembers",
                c => new
                    {
                        Guild_id = c.String(nullable: false, maxLength: 128),
                        Member_id = c.String(nullable: false, maxLength: 128),
                        Id = c.String(
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => new { t.Guild_id, t.Member_id })
                .ForeignKey("dbo.Guilds", t => t.Guild_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Member_id, cascadeDelete: true)
                .Index(t => t.Guild_id)
                .Index(t => t.Member_id)
                .Index(t => t.CreatedAt, clustered: true);
            
            AddColumn("dbo.Messages", "Guild_id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "Guild_id");
            AddForeignKey("dbo.Messages", "Guild_id", "dbo.Guilds", "Id");
            DropColumn("dbo.Messages", "Channel_id");
            DropTable("dbo.ChannelMembers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
            DropTable("dbo.Channels",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        Title = c.String(),
                        Image = c.Binary(),
                        Description = c.String(),
                        Visibility = c.Boolean(nullable: false),
                        Creator_id = c.String(nullable: false, maxLength: 128),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChannelMembers",
                c => new
                    {
                        Channel_id = c.String(nullable: false, maxLength: 128),
                        Member_id = c.String(nullable: false, maxLength: 128),
                        Id = c.String(
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => new { t.Channel_id, t.Member_id });
            
            AddColumn("dbo.Messages", "Channel_id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Messages", "Guild_id", "dbo.Guilds");
            DropForeignKey("dbo.GuildMembers", "Member_id", "dbo.Users");
            DropForeignKey("dbo.GuildMembers", "Guild_id", "dbo.Guilds");
            DropForeignKey("dbo.Guilds", "Leader_id", "dbo.Users");
            DropIndex("dbo.GuildMembers", new[] { "CreatedAt" });
            DropIndex("dbo.GuildMembers", new[] { "Member_id" });
            DropIndex("dbo.GuildMembers", new[] { "Guild_id" });
            DropIndex("dbo.Guilds", new[] { "CreatedAt" });
            DropIndex("dbo.Guilds", new[] { "Leader_id" });
            DropIndex("dbo.Messages", new[] { "Guild_id" });
            DropColumn("dbo.Messages", "Guild_id");
            DropTable("dbo.GuildMembers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
            DropTable("dbo.Guilds",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
            CreateIndex("dbo.Messages", "Channel_id");
            CreateIndex("dbo.Channels", "CreatedAt", clustered: true);
            CreateIndex("dbo.Channels", "Creator_id");
            CreateIndex("dbo.ChannelMembers", "CreatedAt", clustered: true);
            CreateIndex("dbo.ChannelMembers", "Member_id");
            CreateIndex("dbo.ChannelMembers", "Channel_id");
            AddForeignKey("dbo.ChannelMembers", "Member_id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChannelMembers", "Channel_id", "dbo.Channels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "Channel_id", "dbo.Channels", "Id");
            AddForeignKey("dbo.Channels", "Creator_id", "dbo.Users", "Id");
        }
    }
}
