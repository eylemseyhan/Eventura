using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class endmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketCount",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Payments",
                newName: "PaymentStatus");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "EventsTicketId",
                table: "Tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SavedCardId",
                table: "Payments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventsTicketId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventsTickets",
                columns: table => new
                {
                    EventsTicketId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    TicketCapacity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SoldCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsTickets", x => x.EventsTicketId);
                });

            migrationBuilder.CreateTable(
                name: "SavedCards",
                columns: table => new
                {
                    SavedCardId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CardHolderName = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CVV = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedCards", x => x.SavedCardId);
                    table.ForeignKey(
                        name: "FK_SavedCards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventEventsTicket",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    EventsTicketsEventsTicketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEventsTicket", x => new { x.EventId, x.EventsTicketsEventsTicketId });
                    table.ForeignKey(
                        name: "FK_EventEventsTicket_EventsTickets_EventsTicketsEventsTicketId",
                        column: x => x.EventsTicketsEventsTicketId,
                        principalTable: "EventsTickets",
                        principalColumn: "EventsTicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEventsTicket_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventsTicketId",
                table: "Tickets",
                column: "EventsTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SavedCardId",
                table: "Payments",
                column: "SavedCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventEventsTicket_EventsTicketsEventsTicketId",
                table: "EventEventsTicket",
                column: "EventsTicketsEventsTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedCards_UserId",
                table: "SavedCards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SavedCards_SavedCardId",
                table: "Payments",
                column: "SavedCardId",
                principalTable: "SavedCards",
                principalColumn: "SavedCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_EventsTickets_EventsTicketId",
                table: "Tickets",
                column: "EventsTicketId",
                principalTable: "EventsTickets",
                principalColumn: "EventsTicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SavedCards_SavedCardId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_EventsTickets_EventsTicketId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "EventEventsTicket");

            migrationBuilder.DropTable(
                name: "SavedCards");

            migrationBuilder.DropTable(
                name: "EventsTickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventsTicketId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SavedCardId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EventsTicketId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SavedCardId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EventsTicketId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Payments",
                newName: "PaymentMethod");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TicketCount",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
