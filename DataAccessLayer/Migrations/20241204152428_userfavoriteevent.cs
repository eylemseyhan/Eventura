using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class userfavoriteevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Tickets_TicketId",
                table: "UserFavorites");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "UserFavorites",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorites_TicketId",
                table: "UserFavorites",
                newName: "IX_UserFavorites_EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Events_EventId",
                table: "UserFavorites",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Events_EventId",
                table: "UserFavorites");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "UserFavorites",
                newName: "TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorites_EventId",
                table: "UserFavorites",
                newName: "IX_UserFavorites_TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Tickets_TicketId",
                table: "UserFavorites",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
