using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server.Tables;
using ULFGService.DataObjects;

namespace ULFGService
{
    /// <summary>
    /// Representa el modelo de la base de datos
    /// </summary>
    public class ULFGContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=ULFG_ConnectionString";

        /// <summary>
        /// Desactiva el Lazy Loading
        /// </summary>
        public ULFGContext() : base(connectionStringName)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        #region DataObjects
        /// <summary>
        /// Enlace a la tabla de Users
        /// </summary>
        public DbSet<User> UserItems { get; set; }
        /// <summary>
        /// Enlace a la tabla de Messages
        /// </summary>
        public DbSet<Message> MessageItems { get; set; }
        /// <summary>
        /// Enlace a la tabla de Chats
        /// </summary>
        public DbSet<Chat> ChatItems { get; set; }
        /// <summary>
        /// Enlace a la tabla de Guilds
        /// </summary>
        public DbSet<Guild> GuildItems { get; set; }
        /// <summary>
        /// Enlace a la tabla de GuildMembers
        /// </summary>
        public DbSet<GuildMember> GuildMembers { get; set; }
        /// <summary>
        /// Enlace a la tabla de Follows
        /// </summary>
        public DbSet<Follow> Follows { get; set; }
        /// <summary>
        /// Enlace a la tabla de Blocks
        /// </summary>
        public DbSet<Block> Blocks { get; set; }
        /// <summary>
        /// Enlace a la tabla de Publications
        /// </summary>
        public DbSet<Publication> Publications { get; set; }
        #endregion

        /// <summary>
        /// Crea el modelo de la base datos con las restricciones necesarias
        /// </summary>
        /// <param name="modelBuilder">Elemento para mapear las clases a la base de datos</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            modelBuilder.Entity<Guild>()
                 .HasRequired<User>(u => u.Leader)
                 .WithMany(u => u.GuildsOwned)
                 .HasForeignKey<string>(ch => ch.Leader_id)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired<User>(u => u.Creator)
                .WithMany(u => u.Messages)
                .HasForeignKey<string>(m => m.Creator_id);

            modelBuilder.Entity<Message>()
                .HasOptional<Chat>(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey<string>(m => m.Chat_id);

            modelBuilder.Entity<Message>()
              .HasOptional<Guild>(m => m.Guild)
              .WithMany(c => c.Messages)
              .HasForeignKey<string>(m => m.Guild_id);

            modelBuilder.Entity<Chat>()
                .HasOptional<User>(ci => ci.Member1)
                .WithMany(u => u.ChatsM1)
                .HasForeignKey<string>(ci => ci.Member1_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chat>()
               .HasOptional<User>(ci => ci.Member2)
               .WithMany(u => u.ChatsM2)
               .HasForeignKey<string>(ci => ci.Member2_id)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<GuildMember>()
                .HasRequired<Guild>(c => c.Guild)
                .WithMany(u => u.Members)
                .HasForeignKey<string>(c => c.Guild_id);

            modelBuilder.Entity<GuildMember>()
                .HasRequired<User>(c => c.Member)
                .WithMany(u => u.Guilds)
                .HasForeignKey<string>(c => c.Member_id);

            modelBuilder.Entity<Follow>()
               .HasRequired<User>(c => c.User2)
               .WithMany(u => u.Follow2)
               .HasForeignKey<string>(c => c.Followed_user)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Follow>()
              .HasRequired<User>(c => c.User1)
              .WithMany(u => u.Follow1)
              .HasForeignKey<string>(c => c.Following_user)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Block>()
               .HasRequired<User>(c => c.User2)
               .WithMany(u => u.Block2)
               .HasForeignKey<string>(c => c.Blocked_user)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Block>()
              .HasRequired<User>(c => c.User1)
              .WithMany(u => u.Block1)
              .HasForeignKey<string>(c => c.Blocking_user)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publication>()
                .HasRequired<User>(p => p.Autor)
                .WithMany(u => u.Publications)
                .HasForeignKey<string>(p => p.Autor_id);          
        }
    }
}
