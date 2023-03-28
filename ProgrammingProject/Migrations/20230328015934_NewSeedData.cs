using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgrammingProject.Migrations
{
    /// <inheritdoc />
    public partial class NewSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Walker_Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Walker_PhNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInsured = table.Column<bool>(type: "bit", nullable: true),
                    ExperienceLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    LoginID = table.Column<string>(type: "char(8)", maxLength: 8, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "char(64)", maxLength: 64, nullable: false),
                    Locked = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK_Logins_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacesWalked",
                columns: table => new
                {
                    Suburb = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WalkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacesWalked", x => x.Suburb);
                    table.ForeignKey(
                        name: "FK_PlacesWalked_User_WalkerId",
                        column: x => x.WalkerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkerRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerID = table.Column<int>(type: "int", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkerRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkerRatings_User_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WalkerRatings_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WalkingSession",
                columns: table => new
                {
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkingSession", x => x.StartTime);
                    table.ForeignKey(
                        name: "FK_WalkingSession_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MicrochipNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsVaccinated = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    VetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogs_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dogs_Vets_VetId",
                        column: x => x.VetId,
                        principalTable: "Vets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DogRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DogID = table.Column<int>(type: "int", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DogRatings_Dogs_DogID",
                        column: x => x.DogID,
                        principalTable: "Dogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DogRatings_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DogWalkingSession",
                columns: table => new
                {
                    DogListId = table.Column<int>(type: "int", nullable: false),
                    WalkingSessionsStartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogWalkingSession", x => new { x.DogListId, x.WalkingSessionsStartTime });
                    table.ForeignKey(
                        name: "FK_DogWalkingSession_Dogs_DogListId",
                        column: x => x.DogListId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogWalkingSession_WalkingSession_WalkingSessionsStartTime",
                        column: x => x.WalkingSessionsStartTime,
                        principalTable: "WalkingSession",
                        principalColumn: "StartTime",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DogRatings_DogID",
                table: "DogRatings",
                column: "DogID");

            migrationBuilder.CreateIndex(
                name: "IX_DogRatings_WalkerID",
                table: "DogRatings",
                column: "WalkerID");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_OwnerId",
                table: "Dogs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_VetId",
                table: "Dogs",
                column: "VetId");

            migrationBuilder.CreateIndex(
                name: "IX_DogWalkingSession_WalkingSessionsStartTime",
                table: "DogWalkingSession",
                column: "WalkingSessionsStartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserID",
                table: "Logins",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlacesWalked_WalkerId",
                table: "PlacesWalked",
                column: "WalkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkerRatings_OwnerID",
                table: "WalkerRatings",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_WalkerRatings_WalkerID",
                table: "WalkerRatings",
                column: "WalkerID");

            migrationBuilder.CreateIndex(
                name: "IX_WalkingSession_WalkerID",
                table: "WalkingSession",
                column: "WalkerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DogRatings");

            migrationBuilder.DropTable(
                name: "DogWalkingSession");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "PlacesWalked");

            migrationBuilder.DropTable(
                name: "WalkerRatings");

            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "WalkingSession");

            migrationBuilder.DropTable(
                name: "Vets");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
