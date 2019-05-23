using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MyMantriDataAccessLayer.Models
{
    public partial class MyMantriDBContext : DbContext
    {
        public MyMantriDBContext()
        {
        }

        public MyMantriDBContext(DbContextOptions<MyMantriDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Complaint> Complaint { get; set; }
        public virtual DbSet<Constituency> Constituency { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Mantri> Mantri { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserCredentials> UserCredentials { get; set; }
        public virtual DbSet<Voters> Voters { get; set; }
        public virtual DbSet<Workdone> Workdone { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("MyMantriDBConnectionString");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(e => e.EmailId)
                    .HasName("UQ__Admin__B79555BE2DD559AD")
                    .IsUnique();

                entity.Property(e => e.AdminId)
                    .HasColumnName("Admin_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("Email_Id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityAns)
                    .IsRequired()
                    .HasColumnName("Security_Ans")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdminNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Admin__Admin_id__38996AB5");
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.Property(e => e.ComplaintId).HasColumnName("Complaint_ID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ComplaintDateTime)
                    .HasColumnName("Complaint_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Constituency)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.VoterId).HasColumnName("Voter_id");

                entity.HasOne(d => d.ConstituencyNavigation)
                    .WithMany(p => p.Complaint)
                    .HasForeignKey(d => d.Constituency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Complaint__Const__3C69FB99");

                entity.HasOne(d => d.Voter)
                    .WithMany(p => p.Complaint)
                    .HasForeignKey(d => d.VoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Complaint__Voter__3B75D760");
            });

            modelBuilder.Entity<Constituency>(entity =>
            {
                entity.HasKey(e => e.ConstituencyName);

                entity.HasIndex(e => e.MantriUid)
                    .HasName("UQ__Constitu__60BE27A897DAFBBB")
                    .IsUnique();

                entity.Property(e => e.ConstituencyName)
                    .HasColumnName("Constituency_name")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.MantriUid)
                    .IsRequired()
                    .HasColumnName("Mantri_UID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.ComplaintId);

                entity.Property(e => e.ComplaintId)
                    .HasColumnName("Complaint_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FeedbackDesr)
                    .HasColumnName("Feedback_Desr")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Complaint)
                    .WithOne(p => p.Feedback)
                    .HasForeignKey<Feedback>(d => d.ComplaintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__Compla__3F466844");
            });

            modelBuilder.Entity<Mantri>(entity =>
            {
                entity.HasIndex(e => e.Constituency)
                    .HasName("UQ__Mantri__BA17DBFFB92BF884")
                    .IsUnique();
                    
                entity.HasIndex(e => e.EmailId)
                    .HasName("UQ__Mantri__B79555BE2DC920CA")
                    .IsUnique();

                entity.HasIndex(e => e.MantriUid)
                    .HasName("UQ__Mantri__60BE27A8C00F527B")
                    .IsUnique();

                entity.Property(e => e.MantriId)
                    .HasColumnName("Mantri_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Constituency)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("Email_Id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MantriUid)
                    .IsRequired()
                    .HasColumnName("Mantri_UID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityAns)
                    .IsRequired()
                    .HasColumnName("Security_Ans")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstituencyNavigation)
                    .WithOne(p => p.MantriConstituencyNavigation)
                    .HasForeignKey<Mantri>(d => d.Constituency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Mantri__Constitu__2F10007B");

                entity.HasOne(d => d.MantriNavigation)
                    .WithOne(p => p.Mantri)
                    .HasForeignKey<Mantri>(d => d.MantriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Mantri__Mantri_i__2E1BDC42");

                entity.HasOne(d => d.MantriU)
                    .WithOne(p => p.MantriMantriU)
                    .HasPrincipalKey<Constituency>(p => p.MantriUid)
                    .HasForeignKey<Mantri>(d => d.MantriUid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Mantri__Mantri_U__300424B4");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleType)
                    .IsRequired()
                    .HasColumnName("Role_Type")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserCredentials>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("User_Credentials");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.RollId).HasColumnName("Roll_id");

                entity.HasOne(d => d.Roll)
                    .WithMany(p => p.UserCredentials)
                    .HasForeignKey(d => d.RollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Cred__Roll___25869641");
            });

            modelBuilder.Entity<Voters>(entity =>
            {
                entity.HasKey(e => e.VoterId);

                entity.HasIndex(e => e.EmailId)
                    .HasName("UQ__Voters__B79555BEC5A62B31")
                    .IsUnique();

                entity.Property(e => e.VoterId)
                    .HasColumnName("Voter_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Constituency)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("Email_Id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityAns)
                    .IsRequired()
                    .HasColumnName("Security_Ans")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstituencyNavigation)
                    .WithMany(p => p.Voters)
                    .HasForeignKey(d => d.Constituency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Voters__Constitu__34C8D9D1");

                entity.HasOne(d => d.Voter)
                    .WithOne(p => p.Voters)
                    .HasForeignKey<Voters>(d => d.VoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Voters__Voter_id__33D4B598");
            });

            modelBuilder.Entity<Workdone>(entity =>
            {
                entity.HasKey(e => e.WorkId);

                entity.Property(e => e.WorkId).HasColumnName("Work_ID");

                entity.Property(e => e.MantriId).HasColumnName("Mantri_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkDesr)
                    .IsRequired()
                    .HasColumnName("Work_Desr")
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Mantri)
                    .WithMany(p => p.Workdone)
                    .HasForeignKey(d => d.MantriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Workdone__Mantri__4222D4EF");
            });
        }
    }
}
