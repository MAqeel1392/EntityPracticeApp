using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityPracticeApp.Migrations
{
    public class Changes : Migration
    {
        public Changes()
        {
        }

        public override IModel TargetModel => base.TargetModel;

        public override IReadOnlyList<MigrationOperation> UpOperations => base.UpOperations;

        public override IReadOnlyList<MigrationOperation> DownOperations => base.DownOperations;

        public override string? ActiveProvider { get => base.ActiveProvider; set => base.ActiveProvider = value; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            base.BuildTargetModel(modelBuilder);
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<string>(
            name: "DOB",
            table: "Students",
            type: "DateTime",
            nullable: false,
            defaultValue: 0);
            migrationBuilder.AddColumn<string>(
                name: "Created",
                table: "Students",
                type: "DateTime",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<string>(
            name: "Updated",
            table: "Students",
            type: "DateTime",
            nullable: false,
            defaultValue: 0);

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Age", table: "Students");
            migrationBuilder.DropColumn(name: "Description", table: "Students");
        }


    }
}
