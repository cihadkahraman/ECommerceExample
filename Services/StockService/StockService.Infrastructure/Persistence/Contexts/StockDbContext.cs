using MassTransit;
using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence.Contexts
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Name)
                       .IsRequired()
                       .HasMaxLength(100);

                builder.Property(p => p.Description)
                       .HasMaxLength(500);

                builder.OwnsOne(p => p.Price, price =>
                {
                    price.Property(p => p.Amount)
                         .HasColumnName("Price")
                         .HasColumnType("decimal(18,2)");

                    price.Property(p => p.Currency)
                         .HasColumnName("Currency")
                         .HasMaxLength(5);
                });
            });

            // Stock
            modelBuilder.Entity<Stock>(builder =>
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Quantity).IsRequired();

                builder.HasOne(s => s.Product)
                       .WithOne(p => p.Stock)
                       .HasForeignKey<Stock>(s => s.ProductId);
            });

            // MassTransit Outbox Pattern
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
