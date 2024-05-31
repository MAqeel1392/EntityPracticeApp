using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityPracticeApp.Migrations
{
    /// <inheritdoc />
    public partial class EntityFrameworkPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",      
                columns: table => new
                {
                    Sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(name: "Student Name", type: "nvarchar(max)", nullable: false),
                    StudentEmail = table.Column<string>(name: "Student Email", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Sid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
