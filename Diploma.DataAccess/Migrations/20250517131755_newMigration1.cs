using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListPrice",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Price100",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Price50",
                table: "Subjects");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ListPrice",
                table: "Subjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price100",
                table: "Subjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price50",
                table: "Subjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 99.0, 80.0, 85.0 });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 40.0, 20.0, 25.0 });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 55.0, 35.0, 40.0 });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 70.0, 55.0, 60.0 });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 30.0, 20.0, 25.0 });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ListPrice", "Price100", "Price50" },
                values: new object[] { 25.0, 20.0, 22.0 });
        }
    }
}
