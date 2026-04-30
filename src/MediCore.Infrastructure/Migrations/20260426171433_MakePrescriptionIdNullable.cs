using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakePrescriptionIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispensedMedications_Prescriptions_PrescriptionId",
                table: "DispensedMedications");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrescriptionId",
                table: "DispensedMedications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedMedications_Prescriptions_PrescriptionId",
                table: "DispensedMedications",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispensedMedications_Prescriptions_PrescriptionId",
                table: "DispensedMedications");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrescriptionId",
                table: "DispensedMedications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DispensedMedications_Prescriptions_PrescriptionId",
                table: "DispensedMedications",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
