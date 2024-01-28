using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_MVC.Migrations
{
    /// <inheritdoc />
    public partial class xx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategoria",
                columns: table => new
                {
                    IdKategorii = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoria", x => x.IdKategorii);
                });

            migrationBuilder.CreateTable(
                name: "Ranga",
                columns: table => new
                {
                    IdRangi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PotrzebnaLiczbaWiadomosci = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranga", x => x.IdRangi);
                });

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
                name: "Skin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CssPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skin", x => x.Id);
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
                name: "ZakazaneSlowa",
                columns: table => new
                {
                    ZakazaneSlowoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slowo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakazaneSlowa", x => x.ZakazaneSlowoId);
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
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoutTimeSpan = table.Column<int>(type: "int", nullable: true),
                    SkinId = table.Column<int>(type: "int", nullable: false)
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
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiczbaWatkow = table.Column<int>(type: "int", nullable: true),
                    LiczbaWiadomosci = table.Column<int>(type: "int", nullable: true),
                    IdUprawnien = table.Column<int>(type: "int", nullable: false),
                    IdKategorii = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.IdForum);
                    table.ForeignKey(
                        name: "FK_Forum_Kategoria_IdKategorii",
                        column: x => x.IdKategorii,
                        principalTable: "Kategoria",
                        principalColumn: "IdKategorii",
                        onDelete: ReferentialAction.Cascade);
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
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUzytkownika = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogloszenie", x => x.IdOgloszenia);
                    table.ForeignKey(
                        name: "FK_Ogloszenie_User_UserIdUzytkownika",
                        column: x => x.UserIdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika");
                });

            migrationBuilder.CreateTable(
                name: "RangaUzytkownika",
                columns: table => new
                {
                    IdRangiUzytkownika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRangi = table.Column<int>(type: "int", nullable: false),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangaUzytkownika", x => x.IdRangiUzytkownika);
                    table.ForeignKey(
                        name: "FK_RangaUzytkownika_Ranga_IdRangi",
                        column: x => x.IdRangi,
                        principalTable: "Ranga",
                        principalColumn: "IdRangi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RangaUzytkownika_User_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wiadomosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataWyslania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NadawcaId = table.Column<int>(type: "int", nullable: false),
                    OdbiorcaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wiadomosci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wiadomosci_User_NadawcaId",
                        column: x => x.NadawcaId,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wiadomosci_User_OdbiorcaId",
                        column: x => x.OdbiorcaId,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Restrict);
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
                    LiczbaOdwiedzen = table.Column<int>(type: "int", nullable: true),
                    LiczbaOdpowiedzi = table.Column<int>(type: "int", nullable: true),
                    CzyPrzyklejony = table.Column<bool>(type: "bit", nullable: true)
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
                name: "Moderator",
                columns: table => new
                {
                    IdModeratora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false),
                    IdForum = table.Column<int>(type: "int", nullable: false),
                    ForumIdForum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderator", x => x.IdModeratora);
                    table.ForeignKey(
                        name: "FK_Moderator_Forum_ForumIdForum",
                        column: x => x.ForumIdForum,
                        principalTable: "Forum",
                        principalColumn: "IdForum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Moderator_User_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUzytkownika");
                });

            migrationBuilder.CreateTable(
                name: "Odpowiedz",
                columns: table => new
                {
                    OdpowiedzId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataOdpowiedzi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DyskusjaId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    ZalacznikPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZgloszenieModeracji = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odpowiedz", x => x.OdpowiedzId);
                    table.ForeignKey(
                        name: "FK_Odpowiedz_Dyskusja_DyskusjaId",
                        column: x => x.DyskusjaId,
                        principalTable: "Dyskusja",
                        principalColumn: "DyskusjaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odpowiedz_User_AutorId",
                        column: x => x.AutorId,
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
                name: "IX_Forum_IdKategorii",
                table: "Forum",
                column: "IdKategorii");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_IdUprawnien",
                table: "Forum",
                column: "IdUprawnien");

            migrationBuilder.CreateIndex(
                name: "IX_Moderator_ForumIdForum",
                table: "Moderator",
                column: "ForumIdForum");

            migrationBuilder.CreateIndex(
                name: "IX_Moderator_IdUzytkownika",
                table: "Moderator",
                column: "IdUzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Odpowiedz_AutorId",
                table: "Odpowiedz",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Odpowiedz_DyskusjaId",
                table: "Odpowiedz",
                column: "DyskusjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ogloszenie_UserIdUzytkownika",
                table: "Ogloszenie",
                column: "UserIdUzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_RangaUzytkownika_IdRangi",
                table: "RangaUzytkownika",
                column: "IdRangi");

            migrationBuilder.CreateIndex(
                name: "IX_RangaUzytkownika_IdUzytkownika",
                table: "RangaUzytkownika",
                column: "IdUzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRoli",
                table: "User",
                column: "IdRoli");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosci_NadawcaId",
                table: "Wiadomosci",
                column: "NadawcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosci_OdbiorcaId",
                table: "Wiadomosci",
                column: "OdbiorcaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moderator");

            migrationBuilder.DropTable(
                name: "Odpowiedz");

            migrationBuilder.DropTable(
                name: "Ogloszenie");

            migrationBuilder.DropTable(
                name: "RangaUzytkownika");

            migrationBuilder.DropTable(
                name: "Skin");

            migrationBuilder.DropTable(
                name: "Wiadomosci");

            migrationBuilder.DropTable(
                name: "ZakazaneSlowa");

            migrationBuilder.DropTable(
                name: "Dyskusja");

            migrationBuilder.DropTable(
                name: "Ranga");

            migrationBuilder.DropTable(
                name: "Forum");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Kategoria");

            migrationBuilder.DropTable(
                name: "UprawnienieAnonimowych");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
