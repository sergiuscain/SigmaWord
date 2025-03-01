using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SigmaWord.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsAndNewStatisticsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalKnownWords",
                table: "DailyStatistics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalWordsStarted",
                table: "DailyStatistics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DailyWordGoal = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedTheme = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TotalKnownWords",
                table: "DailyStatistics");

            migrationBuilder.DropColumn(
                name: "TotalWordsStarted",
                table: "DailyStatistics");
        }
    }
}
