using FaceRecognition.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.DataLayer.Context
{
    public class CoreContext:DbContext
    {
        public CoreContext() : base("name=CoreContext")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<History> Histories { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<User>();
            userEntity.HasKey(x => x.ID);
            userEntity.Property(x => x.FullName)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired();
            userEntity.Property(x => x.UserName)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired();
            userEntity.Property(x => x.PasswordHash)
                .HasMaxLength(1000)
                .IsRequired();
            userEntity.Property(x => x.Email)
                .HasMaxLength(256)
                .IsOptional();
            userEntity.HasMany(x => x.Repositories)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete();
            userEntity.HasMany(x => x.Histories)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete();
            userEntity.ToTable("User", "fr");

            var repoEntity = modelBuilder.Entity<Repository>();
            repoEntity.HasKey(x => x.ID);
            repoEntity.Property(x => x.SampleImage).HasColumnType("varbinary(max)");
            repoEntity.ToTable("Repository", "fr");

            var historyEntity = modelBuilder.Entity<History>();
            historyEntity.HasKey(x => x.ID);
            historyEntity.HasRequired(x => x.Repository)
                .WithRequiredDependent(x => x.History);
            historyEntity.ToTable("History", "fr");

            
        }
    }
}
