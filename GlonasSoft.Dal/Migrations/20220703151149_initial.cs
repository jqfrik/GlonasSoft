using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlonasSoft.Dal.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultDal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Count_Sign_In = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultDal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sign_In_Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Query = table.Column<Guid>(type: "uuid", nullable: false),
                    Percent = table.Column<int>(type: "integer", nullable: false),
                    ResultId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserDalId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRequests_ResultDal_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ResultDal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRequests_Users_UserDalId",
                        column: x => x.UserDalId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_ResultId",
                table: "UserRequests",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_UserDalId",
                table: "UserRequests",
                column: "UserDalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRequests");

            migrationBuilder.DropTable(
                name: "ResultDal");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
