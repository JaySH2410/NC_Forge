using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationCreateVersionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                    table.UniqueConstraint("AK_Application_Uuid", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "MetaObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjTypeUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ObjUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaObject", x => x.Id);
                    table.UniqueConstraint("AK_MetaObject_ObjUid", x => x.ObjUid);
                    table.ForeignKey(
                        name: "FK_MetaObject_Application_ApplicationUid",
                        column: x => x.ApplicationUid,
                        principalTable: "Application",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaObject_MetaObject_ObjTypeUid",
                        column: x => x.ObjTypeUid,
                        principalTable: "MetaObject",
                        principalColumn: "ObjUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaObjectRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End1Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    End2Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelTypeUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RelUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaObjectRelationship", x => x.Id);
                    table.UniqueConstraint("AK_MetaObjectRelationship_RelUid", x => x.RelUid);
                    table.ForeignKey(
                        name: "FK_MetaObjectRelationship_MetaObject_End1Uid",
                        column: x => x.End1Uid,
                        principalTable: "MetaObject",
                        principalColumn: "ObjUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaObjectRelationship_MetaObject_End2Uid",
                        column: x => x.End2Uid,
                        principalTable: "MetaObject",
                        principalColumn: "ObjUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaObjectRelationship_MetaObject_RelTypeUid",
                        column: x => x.RelTypeUid,
                        principalTable: "MetaObject",
                        principalColumn: "ObjUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Application_Uuid",
                table: "Application",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetaObject_ApplicationUid",
                table: "MetaObject",
                column: "ApplicationUid");

            migrationBuilder.CreateIndex(
                name: "IX_MetaObject_ObjTypeUid",
                table: "MetaObject",
                column: "ObjTypeUid");

            migrationBuilder.CreateIndex(
                name: "IX_MetaObject_ObjUid",
                table: "MetaObject",
                column: "ObjUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_End1Uid",
                table: "MetaObjectRelationship",
                column: "End1Uid");

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_End1Uid_RelTypeUid",
                table: "MetaObjectRelationship",
                columns: new[] { "End1Uid", "RelTypeUid" });

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_End2Uid",
                table: "MetaObjectRelationship",
                column: "End2Uid");

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_End2Uid_RelTypeUid",
                table: "MetaObjectRelationship",
                columns: new[] { "End2Uid", "RelTypeUid" });

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_RelTypeUid",
                table: "MetaObjectRelationship",
                column: "RelTypeUid");

            migrationBuilder.CreateIndex(
                name: "IX_MetaObjectRelationship_RelUid",
                table: "MetaObjectRelationship",
                column: "RelUid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetaObjectRelationship");

            migrationBuilder.DropTable(
                name: "MetaObject");

            migrationBuilder.DropTable(
                name: "Application");
        }
    }
}
