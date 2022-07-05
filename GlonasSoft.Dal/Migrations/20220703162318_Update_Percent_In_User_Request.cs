using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlonasSoft.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Update_Percent_In_User_Request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Percent",
                table: "UserRequests",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "UserRequests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
