using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboardMVC.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_O__Order__4316F928",
                table: "Product_Order");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_O__Produ__440B1D61",
                table: "Product_Order");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_U__Produ__4E88ABD4",
                table: "Product_User");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_U__User___4D94879B",
                table: "Product_User");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_O__Order__4316F928",
                table: "Product_Order",
                column: "Order_ID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Product_O__Produ__440B1D61",
                table: "Product_Order",
                column: "Product_ID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Product_U__Produ__4E88ABD4",
                table: "Product_User",
                column: "Product_ID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Product_U__User___4D94879B",
                table: "Product_User",
                column: "User_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_O__Order__4316F928",
                table: "Product_Order");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_O__Produ__440B1D61",
                table: "Product_Order");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_U__Produ__4E88ABD4",
                table: "Product_User");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_U__User___4D94879B",
                table: "Product_User");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_O__Order__4316F928",
                table: "Product_Order",
                column: "Order_ID",
                principalTable: "Order",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_O__Produ__440B1D61",
                table: "Product_Order",
                column: "Product_ID",
                principalTable: "Product",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_U__Produ__4E88ABD4",
                table: "Product_User",
                column: "Product_ID",
                principalTable: "Product",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_U__User___4D94879B",
                table: "Product_User",
                column: "User_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
