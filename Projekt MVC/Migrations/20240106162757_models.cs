using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_MVC.Migrations
{
    /// <inheritdoc />
    public partial class models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRoli = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.IdRoli);
                });

            migrationBuilder.CreateTable(
                name: "UprawnienieAnonimowych",
                columns: table => new
                {
                    IdUprawnienia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UprawnienieAnonimowych", x => x.IdUprawnienia);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Haslo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdRoli = table.Column<int>(type: "int", nullable: false),
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUzytkownika);
                    table.ForeignKey(
                        name: "FK_User_Role_IdRoli",
                        column: x => x.IdRoli,
                        principalTable: "Role",
                        principalColumn: "IdRoli",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forum",
                columns: table => new
                {
                    IdForum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LiczbaWatkow = table.Column<int>(type: "int", nullable: false),
                    LiczbaWiadomosci = table.Column<int>(type: "int", nullable: false),
                    IdUprawnien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.IdForum);
                    table.ForeignKey(
                        name: "FK_Forum_UprawnienieAnonimowych_IdUprawnien",
                        column: x => x.IdUprawnien,
                        principalTable: "UprawnienieAnonimowych",
                        principalColumn: "IdUprawnienia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ogloszenie",
                columns: table => new
                {
                    IdOgloszenia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogloszenie", x => x.IdOgloszenia);
                    table.ForeignKey(
                        name: "FK_Ogloszenie_User_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dyskusja",
                columns: table => new
                {
                    DyskusjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false),
                    IdForum = table.Column<int>(type: "int", nullable: false),
                    LiczbaOdwiedzen = table.Column<int>(type: "int", nullable: false),
                    LiczbaOdpowiedzi = table.Column<int>(type: "int", nullable: false),
                    CzyPrzyklejony = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dyskusja", x => x.DyskusjaId);
                    table.ForeignKey(
                        name: "FK_Dyskusja_Forum_IdForum",
                        column: x => x.IdForum,
                        principalTable: "Forum",
                        principalColumn: "IdForum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dyskusja_User_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odpowiedzi",
                columns: table => new
                {
                    OdpowiedzId = table.Column<int>(type: "int", nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDyskusji = table.Column<int>(type: "int", nullable: false),
                    ZalacznikPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odpowiedzi", x => x.OdpowiedzId);
                    table.ForeignKey(
                        name: "FK_Odpowiedzi_Dyskusja_IdDyskusji",
                        column: x => x.IdDyskusji,
                        principalTable: "Dyskusja",
                        principalColumn: "DyskusjaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odpowiedzi_User_OdpowiedzId",
                        column: x => x.OdpowiedzId,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dyskusja_IdForum",
                table: "Dyskusja",
                column: "IdForum");

            migrationBuilder.CreateIndex(
                name: "IX_Dyskusja_IdUzytkownika",
                table: "Dyskusja",
                column: "IdUzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_IdUprawnien",
                table: "Forum",
                column: "IdUprawnien");

            migrationBuilder.CreateIndex(
                name: "IX_Odpowiedzi_IdDyskusji",
                table: "Odpowiedzi",
                column: "IdDyskusji");

            migrationBuilder.CreateIndex(
                name: "IX_Ogloszenie_IdUzytkownika",
                table: "Ogloszenie",
                column: "IdUzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRoli",
                table: "User",
                column: "IdRoli");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odpowiedzi");

            migrationBuilder.DropTable(
                name: "Ogloszenie");

            migrationBuilder.DropTable(
                name: "Dyskusja");

            migrationBuilder.DropTable(
                name: "Forum");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UprawnienieAnonimowych");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
