using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Numbridle.Migrations
{
    /// <inheritdoc />
    public partial class lllllloololollool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerTraitDiscovery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TraitKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointsAwarded = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    DiscoveredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTraitDiscovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTraitDiscovery_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTraitDiscovery_PlayerId_TraitKey",
                table: "PlayerTraitDiscovery",
                columns: new[] { "PlayerId", "TraitKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTraitDiscovery");
        }
    }
}
