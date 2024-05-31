using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityPracticeApp.Migrations
{
    /// <inheritdoc />
    public partial class Course : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(name: "Title", type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(name: "Description", type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(name: "CreatedDate", type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(name: "UpdatedDate", type: "datetime2", nullable: false),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
