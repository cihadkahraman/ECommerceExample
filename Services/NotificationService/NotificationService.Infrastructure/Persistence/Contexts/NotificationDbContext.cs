using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence.Contexts
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Customer config
            modelBuilder.Entity<Customer>(builder =>
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(c => c.Email)
                    .HasMaxLength(100);

                builder.Property(c => c.PhoneNumber)
                    .HasMaxLength(20);
            });

            // NotificationLog config
            modelBuilder.Entity<NotificationLog>(builder =>
            {
                builder.HasKey(n => n.Id);

                builder.Property(n => n.CustomerId).IsRequired();

                builder.Property(n => n.Message)
                    .IsRequired()
                    .HasMaxLength(500);

                builder.Property(n => n.Channel)
                    .IsRequired()
                    .HasConversion<string>(); // Enum string olarak saklansın

                builder.Property(n => n.SentAt)
                    .IsRequired();

                builder.HasOne<Customer>()
                       .WithMany()
                       .HasForeignKey(n => n.CustomerId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
