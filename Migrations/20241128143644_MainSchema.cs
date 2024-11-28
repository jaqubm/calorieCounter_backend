using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calorieCounter_backend.Migrations
{
    /// <inheritdoc />
    public partial class MainSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "calorieCounter");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "calorieCounter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Energy = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Carbohydrates = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "calorieCounter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValuesPer = table.Column<float>(type: "real", nullable: false),
                    Energy = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Carbohydrates = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_User_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "calorieCounter",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                schema: "calorieCounter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipe_User_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "calorieCounter",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeProduct",
                schema: "calorieCounter",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeProduct", x => new { x.RecipeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_RecipeProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "calorieCounter",
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeProduct_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "calorieCounter",
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEntry",
                schema: "calorieCounter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecipeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntry_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "calorieCounter",
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserEntry_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "calorieCounter",
                        principalTable: "Recipe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserEntry_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "calorieCounter",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_OwnerId",
                schema: "calorieCounter",
                table: "Product",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_OwnerId",
                schema: "calorieCounter",
                table: "Recipe",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProduct_ProductId",
                schema: "calorieCounter",
                table: "RecipeProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "calorieCounter",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntry_ProductId",
                schema: "calorieCounter",
                table: "UserEntry",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntry_RecipeId",
                schema: "calorieCounter",
                table: "UserEntry",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntry_UserId",
                schema: "calorieCounter",
                table: "UserEntry",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeProduct",
                schema: "calorieCounter");

            migrationBuilder.DropTable(
                name: "UserEntry",
                schema: "calorieCounter");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "calorieCounter");

            migrationBuilder.DropTable(
                name: "Recipe",
                schema: "calorieCounter");

            migrationBuilder.DropTable(
                name: "User",
                schema: "calorieCounter");
        }
    }
}
