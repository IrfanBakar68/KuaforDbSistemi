using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforDbSistemi.Migrations
{
    public partial class RecreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eğer tablo varsa silmeden önce kontrol ediyoruz.
            migrationBuilder.Sql("IF OBJECT_ID('Islemler', 'U') IS NOT NULL DROP TABLE [Islemler];");
            migrationBuilder.Sql("IF OBJECT_ID('Salonlar', 'U') IS NOT NULL DROP TABLE [Salonlar];");

            // Salonlar tablosunu oluşturma
            migrationBuilder.CreateTable(
                name: "Salonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalismaSaatleri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonlar", x => x.Id);
                });

            // Islemler tablosunu oluşturma
            migrationBuilder.CreateTable(
                name: "Islemler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ucret = table.Column<double>(type: "float", nullable: false),
                    Sure = table.Column<int>(type: "int", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Islemler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Islemler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Islemler_SalonId",
                table: "Islemler",
                column: "SalonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Islemler tablosunu silme
            migrationBuilder.DropTable("Islemler");

            // Salonlar tablosunu silme
            migrationBuilder.DropTable("Salonlar");
        }
    }
}
