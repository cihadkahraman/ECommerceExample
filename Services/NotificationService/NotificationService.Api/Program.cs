using NotificationService.Infrastructure;
using NotificationService.Application.DependencyInjection;
using Serilog;
using CorrelationId.DependencyInjection;
using CorrelationId;
using Serilog.Sinks.Graylog.Core.Transport;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.PostgreSQL;
using NotificationService.Api.Middlewares;
using NpgsqlTypes;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // Configure Serilog
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "Message", new RenderedMessageColumnWriter() },
            { "TimeStamp", new TimestampColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "CorrelationId", new SinglePropertyColumnWriter("CorrelationId") },
            { "Properties", new LogEventSerializedColumnWriter() }
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("MassTransit", Serilog.Events.LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("Application", "NotificationService")
            .WriteTo.Graylog(new GraylogSinkOptions
            {
                HostnameOrAddress = "localhost",
                Port = 12201,
                TransportType = TransportType.Udp,
                Facility = "notification-service"
            })
            .WriteTo.PostgreSQL(
                connectionString: builder.Configuration.GetConnectionString("NotificationConnection"),
                tableName: "Logs",
                columnOptions: columnWriters,
                needAutoCreateTable: true
            )
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

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(80); // container içi 80 portunu dinle
        });



        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCorrelationId();

        app.UseAuthorization();

        app.MapControllers();

        app.UseSerilogRequestLogging();

        app.UseMiddleware<NotificationService.Api.Middlewares.CorrelationIdMiddleware>();

        app.Run();
    }
}