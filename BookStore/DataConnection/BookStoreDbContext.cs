using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace BookStore.DataConnection
{
    public class BookStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<ApplicationUserOTP> ApplicationUserOTPs { get; set; }
        public DbSet<Author> Authors{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ApplicationUserPromotionCode> ApplicationUserPromotionCodes { get; set; }
        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=FLOOPV\\SQLEXPRESS;initial catalog = ECommerce ;Integrated Security=True;" +
        //        "Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;" +
        //        "Multi Subnet Failover=False");
        //}


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderBook>()
                .HasKey(ob => new { ob.OrderId, ob.BookId });
            modelBuilder.Entity<ApplicationUserPromotionCode>().HasKey(ap => new { ap.ApplicationUserId, ap.PromotionCodeId } );
            modelBuilder.Entity<Cart>().HasKey(c => new { c.ApplicationUserId, c.BookId } );
        }
    }
}
