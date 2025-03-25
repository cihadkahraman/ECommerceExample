using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Common;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Persistence.Contexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Ignore<DomainEvent>();

            //Models
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Id)
                      .ValueGeneratedNever();

                entity.Property(o => o.OrderNumber)
                      .ValueGeneratedOnAdd();

                entity.HasIndex(o => o.OrderNumber)
                      .IsUnique();

                entity.OwnsOne(o => o.ShippingAddress).WithOwner();
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id)
                      .ValueGeneratedNever();
                entity.OwnsOne(o => o.Price).WithOwner();
            });

            //Masstransit outboxpattern
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
