using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManjuuInfrastructure.Migrations
{
    public partial class HealthManjuuCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StartToWrokTime = table.Column<DateTime>(nullable: false),
                    StopToWorkTime = table.Column<DateTime>(nullable: false),
                    WorkSpan = table.Column<int>(nullable: true),
                    PresetTimeout = table.Column<int>(nullable: false),
                    PingSendCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobConfigurations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobConfigurations");
        }
    }
}
