using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgrammingProject.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "char(64)", maxLength: 64, nullable: false),
                    Locked = table.Column<int>(type: "int", nullable: false),
                    EmailToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Suburbs",
                columns: table => new
                {
                    Postcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuburbName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suburbs", x => new { x.Postcode, x.SuburbName });
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SuburbPostcode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SuburbName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Walker_StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Walker_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Walker_Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Walker_PhNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInsured = table.Column<bool>(type: "bit", nullable: true),
                    ExperienceLevel = table.Column<int>(type: "int", nullable: true),
                    Walker_SuburbPostcode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Walker_SuburbName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Logins_Email",
                        column: x => x.Email,
                        principalTable: "Logins",
                        principalColumn: "Email");
                    table.ForeignKey(
                        name: "FK_User_Suburbs_SuburbPostcode_SuburbName",
                        columns: x => new { x.SuburbPostcode, x.SuburbName },
                        principalTable: "Suburbs",
                        principalColumns: new[] { "Postcode", "SuburbName" });
                    table.ForeignKey(
                        name: "FK_User_Suburbs_Walker_SuburbPostcode_Walker_SuburbName",
                        columns: x => new { x.Walker_SuburbPostcode, x.Walker_SuburbName },
                        principalTable: "Suburbs",
                        principalColumns: new[] { "Postcode", "SuburbName" });
                });

            migrationBuilder.CreateTable(
                name: "Vets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SuburbPostcode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SuburbName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vets_Suburbs_SuburbPostcode_SuburbName",
                        columns: x => new { x.SuburbPostcode, x.SuburbName },
                        principalTable: "Suburbs",
                        principalColumns: new[] { "Postcode", "SuburbName" });
                });

            migrationBuilder.CreateTable(
                name: "WalkerRatings",
                columns: table => new
                {
                    OwnerID = table.Column<int>(type: "int", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkerRatings", x => new { x.WalkerID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_WalkerRatings_User_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_WalkerRatings_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "WalkingSessions",
                columns: table => new
                {
                    SessionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkingSessions", x => x.SessionID);
                    table.ForeignKey(
                        name: "FK_WalkingSessions_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Walks",
                columns: table => new
                {
                    WalkerId = table.Column<int>(type: "int", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuburbName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walks", x => new { x.WalkerId, x.Postcode });
                    table.ForeignKey(
                        name: "FK_Walks_Suburbs_Postcode_SuburbName",
                        columns: x => new { x.Postcode, x.SuburbName },
                        principalTable: "Suburbs",
                        principalColumns: new[] { "Postcode", "SuburbName" });
                    table.ForeignKey(
                        name: "FK_Walks_User_WalkerId",
                        column: x => x.WalkerId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MicrochipNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsVaccinated = table.Column<bool>(type: "bit", nullable: false),
                    Temperament = table.Column<int>(type: "int", nullable: false),
                    DogSize = table.Column<int>(type: "int", nullable: false),
                    TrainingLevel = table.Column<int>(type: "int", nullable: false),
                    DogImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerUserId = table.Column<int>(type: "int", nullable: true),
                    VetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogs_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Dogs_Vets_VetId",
                        column: x => x.VetId,
                        principalTable: "Vets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DogRatings",
                columns: table => new
                {
                    DogID = table.Column<int>(type: "int", nullable: false),
                    WalkerID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogRatings", x => new { x.DogID, x.WalkerID });
                    table.ForeignKey(
                        name: "FK_DogRatings_Dogs_DogID",
                        column: x => x.DogID,
                        principalTable: "Dogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DogRatings_User_WalkerID",
                        column: x => x.WalkerID,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DogWalkingSession",
                columns: table => new
                {
                    DogListId = table.Column<int>(type: "int", nullable: false),
                    WalkingSessionsSessionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogWalkingSession", x => new { x.DogListId, x.WalkingSessionsSessionID });
                    table.ForeignKey(
                        name: "FK_DogWalkingSession_Dogs_DogListId",
                        column: x => x.DogListId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogWalkingSession_WalkingSessions_WalkingSessionsSessionID",
                        column: x => x.WalkingSessionsSessionID,
                        principalTable: "WalkingSessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DogRatings_WalkerID",
                table: "DogRatings",
                column: "WalkerID");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_OwnerUserId",
                table: "Dogs",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_VetId",
                table: "Dogs",
                column: "VetId");

            migrationBuilder.CreateIndex(
                name: "IX_DogWalkingSession_WalkingSessionsSessionID",
                table: "DogWalkingSession",
                column: "WalkingSessionsSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SuburbPostcode_SuburbName",
                table: "User",
                columns: new[] { "SuburbPostcode", "SuburbName" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Walker_SuburbPostcode_Walker_SuburbName",
                table: "User",
                columns: new[] { "Walker_SuburbPostcode", "Walker_SuburbName" });

            migrationBuilder.CreateIndex(
                name: "IX_Vets_SuburbPostcode_SuburbName",
                table: "Vets",
                columns: new[] { "SuburbPostcode", "SuburbName" });

            migrationBuilder.CreateIndex(
                name: "IX_WalkerRatings_OwnerID",
                table: "WalkerRatings",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_WalkingSessions_WalkerID",
                table: "WalkingSessions",
                column: "WalkerID");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_Postcode_SuburbName",
                table: "Walks",
                columns: new[] { "Postcode", "SuburbName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DogRatings");

            migrationBuilder.DropTable(
                name: "DogWalkingSession");

            migrationBuilder.DropTable(
                name: "WalkerRatings");

            migrationBuilder.DropTable(
                name: "Walks");

            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "WalkingSessions");

            migrationBuilder.DropTable(
                name: "Vets");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Suburbs");
        }
    }
}
