using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Backened.Models
{
    public partial class SignalRChatContext : DbContext
    {
        public SignalRChatContext()
        {}
        public SignalRChatContext(DbContextOptions<SignalRChatContext> options) : base(options)
        { }
        public virtual DbSet<Signup> Signups { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Connect to SQL Server Database
                optionsBuilder.UseSqlServer("Data Source=chatdb.cxix3e27rla7.ap-south-1.rds.amazonaws.com;Initial Catalog=my-database;Persist Security Info=True;User Id=admin;Password=adminpass;MultipleActiveResultSets=true;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Signup>(entity =>
            {
                entity.HasKey(e => e.userId);
                entity.Property(e => e.userId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.userName).IsUnique();
                entity.Property(e => e.userName).HasMaxLength(50);
                entity.Property(e => e.userPassword).HasMaxLength(50);
                entity.HasIndex(e => e.userEmail).IsUnique();
                entity.Property(e => e.userEmail).HasMaxLength(50);
                entity.Property(e => e.loginStatus).HasMaxLength(10);
            });
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.chatId);
                entity.HasIndex(e => new { e.senderId, e.receiverId }).HasName("NonClusteredIndex-20210902-114105");
                entity.Property(e => e.chatId).ValueGeneratedNever();
                entity.Property(e => e.connectionId).HasMaxLength(50);
                entity.Property(e => e.senderId).HasMaxLength(50);
                entity.Property(e => e.receiverId).HasMaxLength(50);
                entity.Property(e => e.messageStatus).HasMaxLength(10);
                entity.Property(e => e.messageDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.userId);
                entity.Property(e => e.userId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.userName).IsUnique();
                entity.Property(e => e.userName).HasMaxLength(50);
                entity.Property(e => e.userPassword).HasMaxLength(50);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
