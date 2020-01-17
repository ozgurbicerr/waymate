namespace WMate1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Entry> Entries { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<UserCredential> UserCredentials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Entry)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserCredential>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.UserCredential)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserCredential>()
                .HasMany(e => e.Entries)
                .WithRequired(e => e.UserCredential)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
