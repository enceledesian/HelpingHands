using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelpingHands.Models
{
    public partial class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContributorDetails> ContributorDetails { get; set; }
        public virtual DbSet<DonationRequirement> DonationRequirement { get; set; }
        public virtual DbSet<MeterialItems> MeterialItems { get; set; }
        public virtual DbSet<Volunteer> Volunteer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=tcp:georgianserver.database.windows.net,1433;Initial Catalog=georgiansql;Persist Security Info=False;User ID=georgianstudent;Password=admin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ContributorDetails>(entity =>
            {
                entity.Property(e => e.RecipientInfo).IsUnicode(false);
            });

            modelBuilder.Entity<DonationRequirement>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.DeliveryStatus).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.HasOne(d => d.MaterialItem)
                    .WithMany(p => p.DonationRequirement)
                    .HasForeignKey(d => d.MaterialItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DonationRequirement_Category");
            });

            modelBuilder.Entity<MeterialItems>(entity =>
            {
                entity.Property(e => e.MererialItemName).IsUnicode(false);
            });

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.HasIndex(e => e.EmailId)
                    .HasName("UQ_Volunteer_Email")
                    .IsUnique();

                entity.HasIndex(e => e.MobileNumber)
                    .HasName("UQ_Volunteer_Mobile")
                    .IsUnique();

                entity.Property(e => e.EmailId).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MobileNumber).IsUnicode(false);

                entity.Property(e => e.VolunteerId).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
