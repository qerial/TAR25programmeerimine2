using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Numbridle.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumberMeanings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Combination = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Points = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ZeroTier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberMeanings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalScore = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumberDiscoveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    NumberMeaningId = table.Column<int>(type: "int", nullable: false),
                    DiscoveredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointsAwarded = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberDiscoveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberDiscoveries_NumberMeanings_NumberMeaningId",
                        column: x => x.NumberMeaningId,
                        principalTable: "NumberMeanings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NumberDiscoveries_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NumberDiscoveries_NumberMeaningId",
                table: "NumberDiscoveries",
                column: "NumberMeaningId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberDiscoveries_PlayerId_NumberMeaningId",
                table: "NumberDiscoveries",
                columns: new[] { "PlayerId", "NumberMeaningId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NumberMeanings_Combination",
                table: "NumberMeanings",
                column: "Combination",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberDiscoveries");

            migrationBuilder.DropTable(
                name: "NumberMeanings");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
