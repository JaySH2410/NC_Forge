using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DBSeederForMetaSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetaObject_Application_ApplicationUid",
                table: "MetaObject");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Application_Uuid",
                table: "Application");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Application",
                table: "Application");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_Application_Uuid",
                table: "Applications",
                newName: "IX_Applications_Uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "MetaObject",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Applications_Uuid",
                table: "Applications",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MetaObject_Applications_ApplicationUid",
                table: "MetaObject",
                column: "ApplicationUid",
                principalTable: "Applications",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetaObject_Applications_ApplicationUid",
                table: "MetaObject");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Applications_Uuid",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_Uuid",
                table: "Application",
                newName: "IX_Application_Uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "MetaObject",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Application_Uuid",
                table: "Application",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Application",
                table: "Application",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MetaObject_Application_ApplicationUid",
                table: "MetaObject",
                column: "ApplicationUid",
                principalTable: "Application",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
