using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSectorPictureUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Sectors");

            // migrationBuilder.AddColumn<string>(
            //     name: "NumSector",
            //     table: "Sectors",
            //     type: "text",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Bolts",
            //     table: "ClimbingRoutes",
            //     type: "text",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Height",
            //     table: "ClimbingRoutes",
            //     type: "text",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "NumRouter",
            //     table: "ClimbingRoutes",
            //     type: "text",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Type",
            //     table: "ClimbingRoutes",
            //     type: "text",
            //     nullable: true);

            // migrationBuilder.CreateTable(
            //     name: "Pictures",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ParentId = table.Column<Guid>(type: "uuid", nullable: true),
            //         EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         Name = table.Column<string>(type: "text", nullable: true),
            //         FilePath = table.Column<string>(type: "text", nullable: true),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Pictures", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Pictures_Sectors_ParentId",
            //             column: x => x.ParentId,
            //             principalTable: "Sectors",
            //             principalColumn: "Id");
            //     });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ParentId",
                table: "Pictures",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "NumSector",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "Bolts",
                table: "ClimbingRoutes");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "ClimbingRoutes");

            migrationBuilder.DropColumn(
                name: "NumRouter",
                table: "ClimbingRoutes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ClimbingRoutes");

            migrationBuilder.AddColumn<string[]>(
                name: "Picture",
                table: "Sectors",
                type: "text[]",
                nullable: true);
        }
    }
}
