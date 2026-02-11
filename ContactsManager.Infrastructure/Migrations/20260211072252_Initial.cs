using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetter = table.Column<bool>(type: "boolean", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "varchar(8)", nullable: true, defaultValue: "ABCD1234")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("3dba976c-b731-4cec-b654-0963e5b22589"), "India" },
                    { new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), "Canada" },
                    { new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), "United Kingdom" },
                    { new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), "Australia" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("a1f4a91e-5e3f-4d72-9e7a-2c4a9f4a1a11"), "45 MG Road, Bengaluru", new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), new DateTime(1990, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahul.mehta@example.com", "Male", "Rahul Mehta", false },
                    { new Guid("bd3a24a4-db32-48b4-9cba-1c746d204e29"), "123 Maple Street, Springfield", new Guid("3dba976c-b731-4cec-b654-0963e5b22589"), new DateTime(1985, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice@example.com", "Female", "Alice Johnson", false },
                    { new Guid("c93c8f92-92c5-4a9b-9c91-7a3d2a1b9e55"), "78 Ocean Drive, Miami", new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), new DateTime(1988, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "sophia.martinez@example.com", "Female", "Sophia Martinez", false },
                    { new Guid("e41c2b9a-2b71-4df1-9e4b-1c3f5a6b7c77"), "12 Orchard Road, Singapore", new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), new DateTime(1995, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.wong@example.com", "Male", "Daniel Wong", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
