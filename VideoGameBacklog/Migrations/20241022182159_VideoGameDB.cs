using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGameBacklog.Migrations
{
    /// <inheritdoc />
    public partial class VideoGameDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Pfp = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TotalXP = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Level = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83FCF79DA63", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "friends",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    friend_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__friends__FA44291A0167E3FB", x => new { x.user_id, x.friend_id });
                    table.ForeignKey(
                        name: "FK__friends__friend___08B54D69",
                        column: x => x.friend_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__friends__user_id__07C12930",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "progressLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PlayTime = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__progress__5E5499A80A7B4FF1", x => x.LogID);
                    table.ForeignKey(
                        name: "FK__progressL__UserI__02FC7413",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_friends_friend_id",
                table: "friends",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_progressLogs_UserID",
                table: "progressLogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ__users__A6FBF31BB4CD516C",
                table: "users",
                column: "GoogleID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friends");

            migrationBuilder.DropTable(
                name: "progressLogs");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
