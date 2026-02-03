using ChatData.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChatData.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Participants>().ToTable("Participants");
            modelBuilder.Entity<File>().ToTable("Files");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
