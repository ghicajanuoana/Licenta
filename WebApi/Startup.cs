using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Implementation;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using WebApi.Configuration;
using WebApi.Controllers;
using WebApi.Hubs;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var corsAppSettings = new CorsSettings();
            Configuration.Bind("CorsSettings", corsAppSettings);
            services.AddSingleton(corsAppSettings);

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            var sqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<InternshipContext>(options => options.UseSqlServer(sqlConnectionStr));
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IDeviceTypeService, DeviceTypeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDeviceReadingTypeService, DeviceReadingTypeService>();
            services.AddScoped<IThresholdService, ThresholdService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IDeviceReadingService, DeviceReadingService>();
            services.AddScoped<IDeviceMaintenanceService, DeviceMaintenanceService>();
            services.AddScoped<IExportToExcelService, ExportToExcelService>();
            services.AddTransient<IDeviceMaintenanceRepository, DeviceMaintenanceRepository>();
            services.AddTransient<IDeviceTypeRepository, DeviceTypeRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<IDeviceReadingTypeRepository, DeviceReadingTypeRepository>();
            services.AddTransient<IThresholdRepository, ThresholdRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IDeviceReadingRepository,DeviceReadingRepository>();
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RemoteAnalyticsAPI", Version = "v1" });
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RemoteAnalyticsAPI v1"));
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            UpdateDatabase(app);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AlertHub>("/hubs/alertCount");
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<InternshipContext>())
                {
                    if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }
    }
}
