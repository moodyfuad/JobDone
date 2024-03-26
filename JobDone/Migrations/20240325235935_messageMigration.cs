using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobDone.Migrations
{
    /// <inheritdoc />
    public partial class messageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
            name: "MessageModel",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"), // Add this line to make the column an identity column
                MessageContent = table.Column<string>(nullable: false),
                MessageDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                WhoSendMessage = table.Column<int>(nullable: false),
                CustomerId = table.Column<int>(nullable: false),
                SellerId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Message", x => x.Id);
                table.ForeignKey(
                    name: "FK_Message_Customer_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "CustomerModel",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Message_Seller_SellerId",
                    column: x => x.SellerId,
                    principalTable: "SellerModel",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
                name: "IX_Message_CustomerId",
                table: "MessageModel",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SellerId",
                table: "MessageModel",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
