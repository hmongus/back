using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Bookings.Domain.Models.Entities;
using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Model.Entities;
using workstation_back_end.Favorites.Domain.Models.Entities;

namespace workstation_back_end.Shared.Infraestructure.Persistence.Configuration
{
    public class TripMatchContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Experience.Domain.Models.Entities.Experience> Experiences { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Inquiry.Domain.Models.Entities.Inquiry> Inquiries { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<Agency> Agencies { get; set; } 
        
        public DbSet<Tourist> Tourists { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            void ConfigureBaseEntity<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : BaseEntity
            {
                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");
                entity.Property(e => e.IsActive).IsRequired();
            }
            // Experience Entity Configuration
            builder.Entity<Experience.Domain.Models.Entities.Experience>(entity =>
            {
                entity.ToTable("Experiences");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasAnnotation("CheckConstraint", "LEN(Title) > 0");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasAnnotation("CheckConstraint", "LEN(Description) > 0");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Frequencies)
                    .HasMaxLength(100);

                entity.Property(e => e.Duration)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("DECIMAL(10,2)");

                entity.HasIndex(e => e.Title)
                      .IsUnique();
                
                entity.Property(e => e.AgencyUserId).IsRequired(); 
                
                entity.HasOne(e => e.Agency)
                    .WithMany(a => a.Experiences) 
                    .HasForeignKey(e => e.AgencyUserId) 
                    .HasPrincipalKey(a => a.UserId);
                
                ConfigureBaseEntity(entity);
            });
            builder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.Comment).IsRequired().HasMaxLength(1000);
                entity.Property(r => r.ReviewDate).IsRequired().HasColumnType("DATETIME");
                
                entity.Property(r => r.TouristUserId).IsRequired(); 
                entity.Property(r => r.AgencyUserId).IsRequired();  
                
                entity.HasOne(r => r.TouristUser) 
                    .WithMany() 
                    .HasForeignKey(r => r.TouristUserId) 
                    .HasPrincipalKey(u => u.UserId);
                
                entity.HasOne(r => r.Agency) 
                    .WithMany() 
                    .HasForeignKey(r => r.AgencyUserId) 
                    .HasPrincipalKey(a => a.UserId); 

                ConfigureBaseEntity(entity);
            });
            builder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings"); 
                entity.HasKey(b => b.Id);   
                entity.Property(b => b.BookingDate).IsRequired().HasColumnType("DATETIME");
                entity.Property(b => b.NumberOfPeople).IsRequired();
                entity.Property(b => b.Status).IsRequired().HasMaxLength(50);
                entity.Property(b => b.Price).IsRequired(); 
                entity.Property(b => b.Time)
                    .IsRequired()         
                    .HasMaxLength(10); 
                entity.HasOne(b => b.Experience) 
                    .WithMany() 
                    .HasForeignKey(b => b.ExperienceId) 
                    .IsRequired(); 
                ConfigureBaseEntity(entity);
                entity.Property(b => b.TouristId)
                    .IsRequired(); 
                entity.HasOne(b => b.Tourist)
                    .WithMany() 
                    .HasForeignKey(b => b.TouristId);
                ConfigureBaseEntity(entity);
            }); 
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(c => c.Name).IsUnique();
            });

            builder.Entity<Experience.Domain.Models.Entities.Experience>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Experiences)
                .HasForeignKey(e => e.CategoryId);
            
            
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired();

              
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Number).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(500);


                entity.HasIndex(e => e.Email).IsUnique();

                // Relación 1:1 User <-> Agency
                entity.HasOne(u => u.Agency)
                    .WithOne(a => a.User)
                    .HasForeignKey<Agency>(a => a.UserId);

                // Relación 1:1 User <-> Tourist
                entity.HasOne(u => u.Tourist)
                    .WithOne(t => t.User)
                    .HasForeignKey<Tourist>(t => t.UserId);
                ConfigureBaseEntity(entity);
            });

            // Agency
            builder.Entity<Agency>(entity =>
            {
                entity.ToTable("Agencies");
                
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired(); 
                

                entity.Property(e => e.AgencyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Ruc).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Rating);
                entity.Property(e => e.ReviewCount);
                entity.Property(e => e.ReservationCount);
                entity.Property(e => e.AvatarUrl).HasMaxLength(700);
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);
                entity.Property(e => e.SocialLinkFacebook).HasMaxLength(100);
                entity.Property(e => e.SocialLinkInstagram).HasMaxLength(100);
                entity.Property(e => e.SocialLinkWhatsapp).HasMaxLength(100);
                ConfigureBaseEntity(entity); 
                
                entity.HasOne(a => a.User)
                    .WithOne(u => u.Agency)
                    .HasForeignKey<Agency>(a => a.UserId) 
                    .HasPrincipalKey<User>(u => u.UserId); 
            });


            // Tourist
            builder.Entity<Tourist>(entity =>
            {
                entity.ToTable("Tourists");
                
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired(); 
                

                entity.Property(e => e.AvatarUrl).HasMaxLength(255);
                ConfigureBaseEntity(entity);
                
                entity.HasOne(t => t.User)
                    .WithOne(u => u.Tourist)
                    .HasForeignKey<Tourist>(t => t.UserId) 
                    .HasPrincipalKey<User>(u => u.UserId); 
            });
            
            //Inquiry
            builder.Entity<Inquiry.Domain.Models.Entities.Inquiry>(entity =>
            {
                entity.ToTable("Inquiries");

                entity.HasKey(i => i.Id);

                entity.Property(i => i.Question).HasMaxLength(500).IsRequired();
                entity.Property(i => i.AskedAt).IsRequired().HasColumnType("datetime");

                entity.HasOne(i => i.User)
                    .WithMany()
                    .HasForeignKey(i => i.UserId);

                entity.HasOne(i => i.Experience)
                    .WithMany()
                    .HasForeignKey(i => i.ExperienceId);

                entity.HasOne(i => i.Response)
                    .WithOne(r => r.Inquiry)
                    .HasForeignKey<Response>(r => r.InquiryId);
                ConfigureBaseEntity(entity);
            });

            builder.Entity<Response>(entity =>
            {
                entity.ToTable("Responses");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Answer).HasMaxLength(500).IsRequired();
                entity.Property(r => r.AnsweredAt).IsRequired().HasColumnType("datetime");

                entity.Property(r => r.ResponderId)
                    .HasColumnType("char(36)") 
                    .IsRequired();

                entity.HasOne(r => r.Responder)
                    .WithMany()
                    .HasForeignKey(r => r.ResponderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Inquiry)
                    .WithOne(i => i.Response)
                    .HasForeignKey<Response>(r => r.InquiryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(r => r.InquiryId).IsUnique(); 
                ConfigureBaseEntity(entity);
            });
            
            builder.Entity<ExperienceImage>(entity =>
            {
                entity.ToTable("ExperienceImages");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(e => e.Experience)
                    .WithMany(exp => exp.ExperienceImages)
                    .HasForeignKey(e => e.ExperienceId);

                ConfigureBaseEntity(entity); 
            });

            builder.Entity<Include>(entity =>
            {
                entity.ToTable("Includes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description).IsRequired();

                entity.HasOne(e => e.Experience)
                    .WithMany(exp => exp.Includes)
                    .HasForeignKey(e => e.ExperienceId);

                ConfigureBaseEntity(entity); 
            });
            builder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorites");
                entity.HasKey(f => f.Id);

                entity.Property(f => f.TouristId).IsRequired();
                entity.Property(f => f.ExperienceId).IsRequired();

                entity.HasOne(f => f.Tourist)
                    .WithMany()
                    .HasForeignKey(f => f.TouristId)
                    .HasPrincipalKey(t => t.UserId);

                entity.HasOne(f => f.Experience)
                    .WithMany()
                    .HasForeignKey(f => f.ExperienceId)
                    .HasPrincipalKey(e => e.Id);

                ConfigureBaseEntity(entity);
            });

            builder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedules");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Time).IsRequired();

                entity.HasOne(e => e.Experience)
                    .WithMany(exp => exp.Schedules)
                    .HasForeignKey(e => e.ExperienceId);

                ConfigureBaseEntity(entity); 
            });
        }
        
        
       
    }
}