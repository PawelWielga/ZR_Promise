using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAudit.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitEpochTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DiscoveredAt",
                table: "Programs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DiscoveredAt",
                table: "Programs",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }
    }
}
