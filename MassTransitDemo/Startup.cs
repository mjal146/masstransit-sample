using MassTransit;
using MassTransitDemo.Consumer;
using MassTransitDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MassTransitDemo;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDb"));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProcessUploadedItemConsumer>();
            x.AddConsumer<SaveToDatabaseConsumer>();
            x.AddConsumer<SuccessConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("process-file-queue",
                    ep => { ep.ConfigureConsumer<ProcessUploadedItemConsumer>(context); });

                cfg.ReceiveEndpoint("save-db-queue", ep => { ep.ConfigureConsumer<SaveToDatabaseConsumer>(context); });

                cfg.ReceiveEndpoint("success-queue",
                    ep => { ep.ConfigureConsumer<SuccessConsumer>(context); });
            });
        });

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1"
            });
        });
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        });

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}