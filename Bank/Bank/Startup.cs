using Bank.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;

namespace Bank
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BankAppDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Scoped);
            services.AddControllersWithViews();

            #region time service

            var path = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey)
                + Path.DirectorySeparatorChar.ToString()
                + "Models"
                + Path.DirectorySeparatorChar.ToString()
                + "Services"
                + Path.DirectorySeparatorChar.ToString()
                + "TimeServiceData"
                + Path.DirectorySeparatorChar.ToString()
                + "time";

            string timeshiftData;
            try
            {
                timeshiftData = File.ReadAllText(path);
            }
            catch
            {
                timeshiftData = "";
            }
            DateTime date;
            var strings = timeshiftData.Split(" ");
            if (strings.Length != 3 || !int.TryParse(strings[0], out _)
                || !int.TryParse(strings[1], out _)
                || !int.TryParse(strings[2], out _))
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                date = new DateTime(int.Parse(strings[0]), int.Parse(strings[1]), int.Parse(strings[2]));
            }
            var ts = new TimeService(path, date);
            services.AddSingleton(ts);

            #endregion

            services.AddScoped(
                (IServiceProvider sp) =>
                {
                    var db = sp.GetRequiredService<BankAppDbContext>();
                    var timeService = sp.GetRequiredService<TimeService>();
                    return new FlowService(new ISkippable[]
                    {
                        new CreditFlowHandler(new CreditDbEntityRetriever(db), timeService, db),
                        new DepositFlowHandler(new DepositDbEntityRetriever(db), timeService, db)
                    }, timeService);
                });
        } 

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateFormat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateFormat;

            var supportedCultures = new[]
            {
                culture
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Person}/{action=Index}/{id?}");
            });
        }
    }
}
