using MassTransit;
using Microsoft.EntityFrameworkCore;
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
            //Models
            modelBuilder.Entity<Order>(builder =>
            {
                builder.OwnsOne(o => o.ShippingAddress).WithOwner();
            });
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.OwnsOne(o => o.Price).WithOwner();
            });

            //Fluent Api
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);


            //Masstransit outboxpattern
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
