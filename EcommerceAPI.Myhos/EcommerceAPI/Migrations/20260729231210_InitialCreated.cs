using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Electronic devices and gadgets", "Electronics" },
                    { 2, "Household items", "Home" },
                    { 3, "Apparel and garments", "Clothing" },
                    { 4, "Sports equipment", "Sports" },
                    { 5, "Printed and digital books", "Books" },
                    { 6, "Toys and games for kids", "Toys" },
                    { 7, "Beauty and personal care products", "Beauty" },
                    { 8, "Food and household consumables", "Groceries" },
                    { 9, "Home and office furniture", "Furniture" },
                    { 10, "Car parts and accessories", "Automotive" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "SaleDate", "Status", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 845.00m },
                    { 2, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1270.00m },
                    { 3, new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 411.00m },
                    { 4, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 630.00m },
                    { 5, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 330.00m },
                    { 6, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 572.00m },
                    { 7, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 295.00m },
                    { 8, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 195.00m },
                    { 9, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 489.00m },
                    { 10, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 529.00m },
                    { 11, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 255.00m },
                    { 12, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 89.00m },
                    { 13, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 217.00m },
                    { 14, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1090.00m },
                    { 15, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 45.00m },
                    { 16, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 570.00m },
                    { 17, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 401.00m },
                    { 18, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 780.00m },
                    { 19, new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 420.00m },
                    { 20, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 690.00m },
                    { 21, new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 435.00m },
                    { 22, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 832.00m },
                    { 23, new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 190.00m },
                    { 24, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 445.00m },
                    { 25, new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 116.00m },
                    { 26, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 386.00m },
                    { 27, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1565.00m },
                    { 28, new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 195.00m },
                    { 29, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1300.00m },
                    { 30, new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 335.00m },
                    { 31, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 662.00m },
                    { 32, new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 468.00m },
                    { 33, new DateTime(2026, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 430.00m },
                    { 34, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 190.00m },
                    { 35, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 76.00m },
                    { 36, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3240.00m },
                    { 37, new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 515.00m },
                    { 38, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 480.00m },
                    { 39, new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 280.00m },
                    { 40, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1070.00m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Laptop 8GB RAM, 256GB SSD", "HP Laptop 15", 700.00m, 15 },
                    { 2, 1, "Wireless headphones with noise cancellation", "Bluetooth Headphones", 45.00m, 40 },
                    { 3, 1, "4K UHD Television", "50\" Smart TV", 450.00m, 10 },
                    { 4, 1, "Ergonomic wireless mouse", "Wireless Mouse", 20.00m, 55 },
                    { 5, 1, "RGB backlit mechanical keyboard", "Mechanical Keyboard", 65.00m, 30 },
                    { 6, 1, "10000mAh power bank", "Portable Charger", 25.00m, 45 },
                    { 7, 1, "Fitness tracking smartwatch", "Smartwatch", 120.00m, 20 },
                    { 8, 1, "10-inch tablet, 64GB storage", "Tablet 10 inch", 250.00m, 12 },
                    { 9, 2, "3-speed blender", "Oster Blender", 55.00m, 25 },
                    { 10, 2, "100% cotton sheets, queen size", "Bed Sheet Set", 30.00m, 30 },
                    { 11, 2, "Bagless vacuum cleaner", "Vacuum Cleaner", 90.00m, 18 },
                    { 12, 2, "12-cup drip coffee maker", "Coffee Maker", 40.00m, 22 },
                    { 13, 2, "5.5L digital air fryer", "Air Fryer", 70.00m, 20 },
                    { 14, 2, "LED desk lamp with adjustable arm", "Table Lamp", 22.00m, 35 },
                    { 15, 3, "Breathable t-shirt, size M", "Sports T-Shirt", 15.00m, 60 },
                    { 16, 3, "Straight-fit blue jeans", "Classic Jeans", 40.00m, 35 },
                    { 17, 3, "Unisex rain jacket", "Waterproof Jacket", 65.00m, 20 },
                    { 18, 3, "Lightweight running shoes", "Running Shoes", 55.00m, 28 },
                    { 19, 3, "Insulated winter coat", "Winter Coat", 85.00m, 15 },
                    { 20, 3, "Adjustable cotton cap", "Baseball Cap", 12.00m, 50 },
                    { 21, 4, "Professional ball, size 5", "Soccer Ball", 25.00m, 50 },
                    { 22, 4, "29-inch wheel bike, disc brakes", "Mountain Bike", 300.00m, 8 },
                    { 23, 4, "Non-slip exercise yoga mat", "Yoga Mat", 18.00m, 40 },
                    { 24, 4, "Adjustable dumbbell set 20kg", "Dumbbell Set", 75.00m, 15 },
                    { 25, 4, "Lightweight aluminum tennis racket", "Tennis Racket", 45.00m, 20 },
                    { 26, 5, "Bestselling fiction paperback", "Fiction Novel", 14.00m, 45 },
                    { 27, 5, "Illustrated recipe cookbook", "Cookbook", 22.00m, 25 },
                    { 28, 5, "Illustrated storybook for kids", "Children's Storybook", 10.00m, 40 },
                    { 29, 5, "Personal development guide", "Self-Help Guide", 16.00m, 30 },
                    { 30, 6, "200-piece building blocks", "Building Blocks Set", 28.00m, 35 },
                    { 31, 6, "1:16 scale RC car", "Remote Control Car", 35.00m, 20 },
                    { 32, 6, "1000-piece jigsaw puzzle", "Puzzle 1000 pieces", 15.00m, 30 },
                    { 33, 6, "Family strategy board game", "Board Game", 30.00m, 25 },
                    { 34, 7, "Hydrating facial moisturizer", "Facial Moisturizer", 18.00m, 40 },
                    { 35, 7, "Nourishing shampoo 500ml", "Shampoo", 10.00m, 55 },
                    { 36, 7, "Eau de parfum 100ml", "Perfume", 60.00m, 15 },
                    { 37, 9, "Compact wooden office desk", "Office Desk", 150.00m, 10 },
                    { 38, 9, "Adjustable ergonomic office chair", "Ergonomic Chair", 180.00m, 10 },
                    { 39, 10, "Dashboard car phone holder", "Car Phone Mount", 12.00m, 45 },
                    { 40, 10, "Synthetic motor oil, 5 liters", "Motor Oil 5L", 35.00m, 30 }
                });

            migrationBuilder.InsertData(
                table: "SaleDetails",
                columns: new[] { "Id", "ProductId", "Quantity", "SaleId", "Subtotal", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 8, 2, 1, 500.00m, 250.00m },
                    { 2, 2, 2, 1, 90.00m, 45.00m },
                    { 3, 18, 1, 1, 55.00m, 55.00m },
                    { 4, 16, 5, 1, 200.00m, 40.00m },
                    { 5, 28, 2, 2, 20.00m, 10.00m },
                    { 6, 3, 2, 2, 900.00m, 450.00m },
                    { 7, 2, 5, 2, 225.00m, 45.00m },
                    { 8, 6, 5, 2, 125.00m, 25.00m },
                    { 9, 13, 4, 3, 280.00m, 70.00m },
                    { 10, 35, 5, 3, 50.00m, 10.00m },
                    { 11, 27, 3, 3, 66.00m, 22.00m },
                    { 12, 15, 1, 3, 15.00m, 15.00m },
                    { 13, 28, 3, 4, 30.00m, 10.00m },
                    { 14, 22, 2, 4, 600.00m, 300.00m },
                    { 15, 7, 1, 5, 120.00m, 120.00m },
                    { 16, 6, 3, 5, 75.00m, 25.00m },
                    { 17, 25, 3, 5, 135.00m, 45.00m },
                    { 18, 3, 1, 6, 450.00m, 450.00m },
                    { 19, 30, 4, 6, 112.00m, 28.00m },
                    { 20, 35, 1, 6, 10.00m, 10.00m },
                    { 21, 40, 2, 7, 70.00m, 35.00m },
                    { 22, 24, 1, 7, 75.00m, 75.00m },
                    { 23, 37, 1, 7, 150.00m, 150.00m },
                    { 24, 19, 2, 8, 170.00m, 85.00m },
                    { 25, 6, 1, 8, 25.00m, 25.00m },
                    { 26, 30, 3, 9, 84.00m, 28.00m },
                    { 27, 24, 3, 9, 225.00m, 75.00m },
                    { 28, 11, 2, 9, 180.00m, 90.00m },
                    { 29, 5, 5, 10, 325.00m, 65.00m },
                    { 30, 39, 2, 10, 24.00m, 12.00m },
                    { 31, 11, 2, 10, 180.00m, 90.00m },
                    { 32, 18, 3, 11, 165.00m, 55.00m },
                    { 33, 36, 1, 11, 60.00m, 60.00m },
                    { 34, 15, 2, 11, 30.00m, 15.00m },
                    { 35, 21, 3, 12, 75.00m, 25.00m },
                    { 36, 26, 1, 12, 14.00m, 14.00m },
                    { 37, 21, 4, 13, 100.00m, 25.00m },
                    { 38, 14, 2, 13, 44.00m, 22.00m },
                    { 39, 32, 3, 13, 45.00m, 15.00m },
                    { 40, 26, 2, 13, 28.00m, 14.00m },
                    { 41, 36, 4, 14, 240.00m, 60.00m },
                    { 42, 35, 5, 14, 50.00m, 10.00m },
                    { 43, 17, 4, 14, 260.00m, 65.00m },
                    { 44, 38, 3, 14, 540.00m, 180.00m },
                    { 45, 33, 1, 15, 30.00m, 30.00m },
                    { 46, 32, 1, 15, 15.00m, 15.00m },
                    { 47, 10, 4, 16, 120.00m, 30.00m },
                    { 48, 11, 5, 16, 450.00m, 90.00m },
                    { 49, 25, 5, 17, 225.00m, 45.00m },
                    { 50, 39, 3, 17, 36.00m, 12.00m },
                    { 51, 30, 5, 17, 140.00m, 28.00m },
                    { 52, 8, 3, 18, 750.00m, 250.00m },
                    { 53, 35, 3, 18, 30.00m, 10.00m },
                    { 54, 28, 1, 19, 10.00m, 10.00m },
                    { 55, 11, 3, 19, 270.00m, 90.00m },
                    { 56, 30, 5, 19, 140.00m, 28.00m },
                    { 57, 33, 3, 20, 90.00m, 30.00m },
                    { 58, 7, 5, 20, 600.00m, 120.00m },
                    { 59, 10, 2, 21, 60.00m, 30.00m },
                    { 60, 24, 5, 21, 375.00m, 75.00m },
                    { 61, 1, 1, 22, 700.00m, 700.00m },
                    { 62, 39, 1, 22, 12.00m, 12.00m },
                    { 63, 21, 3, 22, 75.00m, 25.00m },
                    { 64, 32, 3, 22, 45.00m, 15.00m },
                    { 65, 16, 1, 23, 40.00m, 40.00m },
                    { 66, 37, 1, 23, 150.00m, 150.00m },
                    { 67, 5, 2, 24, 130.00m, 65.00m },
                    { 68, 35, 4, 24, 40.00m, 10.00m },
                    { 69, 9, 5, 24, 275.00m, 55.00m },
                    { 70, 34, 2, 25, 36.00m, 18.00m },
                    { 71, 39, 5, 25, 60.00m, 12.00m },
                    { 72, 28, 2, 25, 20.00m, 10.00m },
                    { 73, 26, 5, 26, 70.00m, 14.00m },
                    { 74, 24, 4, 26, 300.00m, 75.00m },
                    { 75, 29, 1, 26, 16.00m, 16.00m },
                    { 76, 5, 1, 27, 65.00m, 65.00m },
                    { 77, 22, 5, 27, 1500.00m, 300.00m },
                    { 78, 38, 1, 28, 180.00m, 180.00m },
                    { 79, 15, 1, 28, 15.00m, 15.00m },
                    { 80, 4, 3, 29, 60.00m, 20.00m },
                    { 81, 15, 1, 29, 15.00m, 15.00m },
                    { 82, 5, 5, 29, 325.00m, 65.00m },
                    { 83, 3, 2, 29, 900.00m, 450.00m },
                    { 84, 32, 5, 30, 75.00m, 15.00m },
                    { 85, 14, 5, 30, 110.00m, 22.00m },
                    { 86, 35, 4, 30, 40.00m, 10.00m },
                    { 87, 9, 2, 30, 110.00m, 55.00m },
                    { 88, 27, 1, 31, 22.00m, 22.00m },
                    { 89, 13, 4, 31, 280.00m, 70.00m },
                    { 90, 7, 3, 31, 360.00m, 120.00m },
                    { 91, 30, 1, 32, 28.00m, 28.00m },
                    { 92, 4, 4, 32, 80.00m, 20.00m },
                    { 93, 7, 3, 32, 360.00m, 120.00m },
                    { 94, 16, 2, 33, 80.00m, 40.00m },
                    { 95, 13, 5, 33, 350.00m, 70.00m },
                    { 96, 28, 3, 34, 30.00m, 10.00m },
                    { 97, 12, 4, 34, 160.00m, 40.00m },
                    { 98, 29, 1, 35, 16.00m, 16.00m },
                    { 99, 36, 1, 35, 60.00m, 60.00m },
                    { 100, 1, 4, 36, 2800.00m, 700.00m },
                    { 101, 6, 4, 36, 100.00m, 25.00m },
                    { 102, 16, 4, 36, 160.00m, 40.00m },
                    { 103, 11, 2, 36, 180.00m, 90.00m },
                    { 104, 4, 1, 37, 20.00m, 20.00m },
                    { 105, 11, 4, 37, 360.00m, 90.00m },
                    { 106, 25, 3, 37, 135.00m, 45.00m },
                    { 107, 19, 4, 38, 340.00m, 85.00m },
                    { 108, 28, 2, 38, 20.00m, 10.00m },
                    { 109, 36, 2, 38, 120.00m, 60.00m },
                    { 110, 4, 5, 39, 100.00m, 20.00m },
                    { 111, 38, 1, 39, 180.00m, 180.00m },
                    { 112, 4, 5, 40, 100.00m, 20.00m },
                    { 113, 38, 5, 40, 900.00m, 180.00m },
                    { 114, 31, 2, 40, 70.00m, 35.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleId",
                table: "SaleDetails",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
