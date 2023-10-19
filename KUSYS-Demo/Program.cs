using AutoMapper;
using KUSYS_Demo.Data;
using KUSYS_Demo.Mapper;
using KUSYS_Demo.Data.Services.Abstract;
using KUSYS_Demo.Data.Services.Concrete;
using KUSYS_Demo.Data.Utilities.Config;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//START EFCore Services and DB Connection
//Connection String
var connectionString = builder.Configuration.GetConnectionString("conn");


builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
 options.UseSqlServer(connectionString),ServiceLifetime.Singleton);

//Account LockOut Service Properties
var lockoutOptions = new LockoutOptions()
{
	AllowedForNewUsers = true,
	DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
	MaxFailedAccessAttempts = 10,

};


//Identity options
builder.Services.AddDefaultIdentity<AppUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequiredLength = 6;
	options.Lockout = lockoutOptions;
}).AddRoles<AppRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders(); ;


//Cookie Settings with security stamp force logout
builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = new PathString("/Views/Home/AccessDenied");
});

//Add Toast Service
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
	ProgressBar = true,
	PositionClass = ToastPositions.TopCenter,
	PreventDuplicates = true,
	CloseButton = true,
});

builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddScoped<ICourseUserService,CourseUserService>();

//Sessions
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(300);
});



//mapper
var mapperConfig = new MapperConfiguration(cfg =>
{
	//mapping profile update edilecek
	cfg.AddProfile(new MappingProfile());
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

var app = builder.Build();

//Configures system for first use
//Adds roles if they doesn't exist
//Adss sytem admin if doesn't exist
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	SeedData.SeedDbData(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

//app.UseMvc(routes =>
//{
//    routes.MapRouteAnalyzer("/routes"); // Add
//    routes.MapRoute(
//        name: "default",
//        template: "{controller}/{action=Index}/{id?}");
//});

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
	);

app.Run();
