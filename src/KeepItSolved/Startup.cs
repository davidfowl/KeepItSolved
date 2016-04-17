using AutoMapper;
using KeepItSolved.Models;
using KeepItSolved.ViewModels;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Threading.Tasks;

namespace KeepItSolved
{
	public class Startup
    {
		public static IConfigurationRoot Configuration;

		public Startup(IApplicationEnvironment appEnv)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}


		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc()
				.AddJsonOptions(opt =>
				{
					opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});

			services.AddIdentity<IdentityUser, IdentityRole>(config =>
			{
				config.User.RequireUniqueEmail = true;
				config.Password.RequiredLength = 6;
				config.Password.RequireDigit = false;
				config.Password.RequireNonLetterOrDigit = false;
				config.Password.RequireUppercase = false;
				config.Password.RequireLowercase = false;
				config.Cookies.ApplicationCookie.LoginPath = "/App/Index";
				config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
				{
					OnRedirectToLogin = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
						{
							ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
						}
						else
						{
							ctx.Response.Redirect(ctx.RedirectUri);
						}
						return Task.FromResult(0);
					}
				};
			})
			.AddEntityFrameworkStores<SolvedContext>();

			services.AddEntityFramework()
				.AddSqlServer()
				.AddDbContext<SolvedContext>();

			services.AddScoped<ISolvedRepository, SolvedRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
			app.UseStaticFiles();

			app.UseIdentity();

			Mapper.Initialize(config =>
			{
				config.CreateMap<Flashcard, FlashcardViewModel>().ReverseMap();
			});

			app.UseMvc( config => 
			{
				config.MapRoute(
					name: "Default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "App", action = "Index" }
					);
			});
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
