using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityPracticeApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPersonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<DateTime>(
            name: "DOB",
            table: "Students",
            type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
            name: "Updated",
            table: "Students",
            type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            


            migrationBuilder.DropColumn(name: "Age", table: "Students");
            migrationBuilder.DropColumn(name: "Description", table: "Students");
        }
    }
}
