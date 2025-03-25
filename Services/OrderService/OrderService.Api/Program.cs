using OrderService.Application.DependencyInjection;
using OrderService.Infrastructure;
using Serilog;
using CorrelationId.DependencyInjection;
using CorrelationId;
namespace OrderService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);


            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<OrderService.Api.Middlewares.CorrelationIdMiddleware>();

            app.UseSerilogRequestLogging();

            app.MapControllers();

            app.Run();
        }
    }
}
