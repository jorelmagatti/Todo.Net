using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialTodoBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var cultura = new CultureInfo("pt-BR");

            migrationBuilder.CreateTable(
                name: "TodoTask".ToString(cultura),
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueBy = table.Column<DateOnly>(type: "date", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User".ToString(cultura),
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var cultura = new CultureInfo("pt-BR");

            migrationBuilder.DropTable(
                name: "TodoTask".ToString(cultura));

            migrationBuilder.DropTable(
                name: "User".ToString(cultura));
        }
    }
}
