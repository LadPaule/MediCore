using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientAssignedDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedDoctorId",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AssignedDoctorId",
                table: "Patients",
                column: "AssignedDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_AssignedDoctorId",
                table: "Patients",
                column: "AssignedDoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_AssignedDoctorId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AssignedDoctorId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AssignedDoctorId",
                table: "Patients");
        }
    }
}
