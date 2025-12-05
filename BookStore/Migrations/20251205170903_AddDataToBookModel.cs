using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Proin at turpis a pede posuere nonummy.', 1, 'Quisque ut erat.', 0.95, 444.45, 1, 'book-8.png', 48.0, 2010, 15);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Nulla nisl.', 4, 'Morbi non quam nec dui luctus rutrum. Nulla tellus.', 1.93, 413.36, 1, 'book-3.jpg', 41.05, 1912, 99);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.', 5, 'Maecenas ut massa quis augue luctus tincidunt.', 2.68, 309.83, 1, 'book-2.png', 45.83, 1921, 20);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc.', 4, 'Quisque ut erat.', 4.75, 328.2, 2, 'book-2.png', 26.01, 2016, 64);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Nulla justo.', 3, 'Suspendisse potenti.', 1.72, 62.85, 1, 'book-2.png', 31.31, 1958, 6);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Praesent lectus.', 1, 'Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.', 0.05, 21.56, 2, 'book-8.png', 96.32, 1954, 64);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Morbi non lectus.', 3, 'Cras in purus eu magna vulputate luctus.', 3.57, 372.59, 3, 'book-8.png', 20.84, 2017, 24);
insert into Books (Title, AuthorId, Description, Rate, Price, CategoryId, Img, Discount, Year, Quantity) values ('Duis at velit eu est congue elementum.', 5, 'Nam dui. Proin leo odio, porttitor id, consequat in, consequat ut, nulla. Sed accumsan felis.', 1.72, 22.7, 1, 'book-3.jpg', 62.72, 1915, 49);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete from Books");
        }
    }
}
