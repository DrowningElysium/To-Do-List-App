using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDo.Data;

namespace ToDo
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                    name: "Home",
                    pattern: "/",
                    defaults: new { controller = "Lists", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "ListIndex",
                    pattern: "Lists",
                    defaults: new { controller = "Lists", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "ListCreate",
                    pattern: "Lists/Create",
                    defaults: new { controller = "Lists", action = "Create" });
                endpoints.MapControllerRoute(
                    name: "ListDetail",
                    pattern: "Lists/{listId:int}",
                    defaults: new { controller = "Lists", action = "Details" });
                endpoints.MapControllerRoute(
                    name: "ListEdit",
                    pattern: "Lists/{listId:int}/Edit",
                    defaults: new { controller = "Lists", action = "Edit" });

                endpoints.MapControllerRoute(
                    name: "ListItemCreate",
                    pattern: "Lists/{listId:int}/Items/Create",
                    defaults: new { controller = "ListItems", action = "Create" });
                endpoints.MapControllerRoute(
                    name: "ListItemComplete",
                    pattern: "Lists/{listId:int}/Items/{itemId:int}/Complete",
                    defaults: new { controller = "ListItems", action = "Complete" });
                endpoints.MapControllerRoute(
                    name: "ListItemEdit",
                    pattern: "Lists/{listId:int}/Items/{itemId:int}/Edit",
                    defaults: new { controller = "ListItems", action = "Edit" });
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
