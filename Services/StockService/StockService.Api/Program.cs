using StockService.Infrastructure;
using StockService.Application.DependencyInjection;
using Serilog;
using CorrelationId;
using CorrelationId.DependencyInjection;

namespace StockService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddDefaultCorrelationId(options =>
            {
                options.RequestHeader = "X-Correlation-ID";
                options.IncludeInResponse = true;
            });

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

            app.UseCorrelationId();


            app.MapControllers();

            app.Run();
        }
    }
}
