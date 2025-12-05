using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToAuthorModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Author (Name, Description) values ('Morbi non quam nec dui luctus rutrum.', 'Vivamus vel nulla eget eros elementum pellentesque.');
insert into Author (Name, Description) values ('Vestibulum quam sapien, varius ut, blandit non, interdum in, ante.', 'Morbi non quam nec dui luctus rutrum.');
insert into Author (Name, Description) values ('Nulla suscipit ligula in lacus.', 'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem. Integer tincidunt ante vel ipsum.');
insert into Author (Name, Description) values ('Donec posuere metus vitae ipsum.', 'In hac habitasse platea dictumst. Etiam faucibus cursus urna.');
insert into Author (Name, Description) values ('Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.', 'Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien.');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
