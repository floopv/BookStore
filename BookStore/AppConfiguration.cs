using BookStore.DataConnection;
using BookStore.Models;
using BookStore.Repos;
using BookStore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BookStore
{
    public static class AppConfiguration
    {
        public static void Config(this IServiceCollection Services , string connectionString)
        {
            Services.AddTransient<IEmailSender, EmailSender>(); 
            Services.AddScoped<IDbInitializer, DbInitializer>();  
            Services.AddScoped<IRepository<ApplicationUserOTP>,Repository<ApplicationUserOTP> >();  
            Services.AddScoped<IRepository<Book>,Repository<Book> >();  
            Services.AddScoped<IRepository<Cart>,Repository<Cart> >();  
            Services.AddScoped<IRepository<Promotion>,Repository<Promotion> >();  
            Services.AddScoped<IRepository<ApplicationUserPromotionCode>,Repository<ApplicationUserPromotionCode> >();  

            Services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });


            Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<BookStoreDbContext>();
        }
    }
}
