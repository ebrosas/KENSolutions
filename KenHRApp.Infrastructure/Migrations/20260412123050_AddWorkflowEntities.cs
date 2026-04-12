using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RequestApprovals",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 377, DateTimeKind.Local).AddTicks(6813),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 550, DateTimeKind.Local).AddTicks(4215));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 421, DateTimeKind.Local).AddTicks(8460),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 573, DateTimeKind.Local).AddTicks(4848));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 420, DateTimeKind.Local).AddTicks(9341),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 572, DateTimeKind.Local).AddTicks(4608));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(6123),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(2848));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 423, DateTimeKind.Local).AddTicks(8779),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 575, DateTimeKind.Local).AddTicks(2679));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 428, DateTimeKind.Local).AddTicks(4709),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 578, DateTimeKind.Local).AddTicks(1232));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1851),
                comment: "Part of composite unique key index",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(937),
                oldComment: "Part of composite unique key index");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1191),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(539));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(6139),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(564));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 12, 12, 30, 48, 420, DateTimeKind.Utc).AddTicks(6769),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 4, 7, 9, 14, 58, 572, DateTimeKind.Utc).AddTicks(142));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 427, DateTimeKind.Local).AddTicks(3490),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(5968));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(9269),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(3329));

            migrationBuilder.CreateTable(
                name: "WorkflowApprovalRoles",
                schema: "kenuser",
                columns: table => new
                {
                    ApprovalRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalGroupCode = table.Column<string>(type: "varchar(50)", nullable: false),
                    ApprovalGroupDesc = table.Column<string>(type: "varchar(200)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(300)", nullable: true),
                    AssigneeEmpNo = table.Column<int>(type: "int", nullable: false),
                    AssigneEmpName = table.Column<string>(type: "varchar(100)", nullable: true),
                    AssigneEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    SubstituteEmpNo = table.Column<int>(type: "int", nullable: true),
                    SubstituteEmpName = table.Column<string>(type: "varchar(100)", nullable: true),
                    SubstituteEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 386, DateTimeKind.Local).AddTicks(1520)),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowApprovalRole_ApprovalRoleId", x => x.ApprovalRoleId);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                schema: "kenuser",
                columns: table => new
                {
                    WorkflowDefinitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    EntityName = table.Column<string>(type: "varchar(100)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDefinition_WorkflowDefinitionId", x => x.WorkflowDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                schema: "kenuser",
                columns: table => new
                {
                    WorkflowInstanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowDefinitionId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstance_WorkflowInstanceId", x => x.WorkflowInstanceId);
                    table.ForeignKey(
                        name: "FK_WorkflowInstances_WorkflowDefinitions_WorkflowDefinitionId",
                        column: x => x.WorkflowDefinitionId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "WorkflowDefinitionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepDefinitions",
                schema: "kenuser",
                columns: table => new
                {
                    StepDefinitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowDefinitionId = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "varchar(200)", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    ApprovalRole = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsParallelGroup = table.Column<bool>(type: "bit", nullable: false),
                    ParallelGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequiresAllParallel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepDefinition_StepDefinitionId", x => x.StepDefinitionId);
                    table.ForeignKey(
                        name: "FK_WorkflowStepDefinitions_WorkflowDefinitions_WorkflowDefinitionId",
                        column: x => x.WorkflowDefinitionId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "WorkflowDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowConditions",
                schema: "kenuser",
                columns: table => new
                {
                    ConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepDefinitionId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Operator = table.Column<string>(type: "varchar(20)", nullable: false),
                    CompareValue = table.Column<string>(type: "varchar(50)", nullable: false),
                    NextStepDefinitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowCondition_ConditionId", x => x.ConditionId);
                    table.ForeignKey(
                        name: "FK_WorkflowConditions_WorkflowStepDefinitions_NextStepDefinitionId",
                        column: x => x.NextStepDefinitionId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowStepDefinitions",
                        principalColumn: "StepDefinitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowConditions_WorkflowStepDefinitions_StepDefinitionId",
                        column: x => x.StepDefinitionId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowStepDefinitions",
                        principalColumn: "StepDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepInstances",
                schema: "kenuser",
                columns: table => new
                {
                    StepInstanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowInstanceId = table.Column<int>(type: "int", nullable: false),
                    StepDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ApproverEmpNo = table.Column<int>(type: "int", nullable: false),
                    ApproverUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproverRole = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    Comments = table.Column<string>(type: "varchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepInstance_StepInstanceId", x => x.StepInstanceId);
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_WorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowInstances",
                        principalColumn: "WorkflowInstanceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_WorkflowStepDefinitions_StepDefinitionId",
                        column: x => x.StepDefinitionId,
                        principalSchema: "kenuser",
                        principalTable: "WorkflowStepDefinitions",
                        principalColumn: "StepDefinitionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalRole_CompoKeys",
                schema: "kenuser",
                table: "WorkflowApprovalRoles",
                column: "ApprovalGroupCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowCondition_CompoKeys",
                schema: "kenuser",
                table: "WorkflowConditions",
                columns: new[] { "StepDefinitionId", "FieldName" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowConditions_NextStepDefinitionId",
                schema: "kenuser",
                table: "WorkflowConditions",
                column: "NextStepDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_UniqueKey",
                schema: "kenuser",
                table: "WorkflowDefinitions",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_CompoKeys",
                schema: "kenuser",
                table: "WorkflowInstances",
                columns: new[] { "WorkflowDefinitionId", "EntityId", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepDefinition_CompoKeys",
                schema: "kenuser",
                table: "WorkflowStepDefinitions",
                columns: new[] { "WorkflowDefinitionId", "StepOrder", "ApprovalRole" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstance_CompoKeys",
                schema: "kenuser",
                table: "WorkflowStepInstances",
                columns: new[] { "WorkflowInstanceId", "StepDefinitionId", "ApproverEmpNo", "ApproverRole" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstances_StepDefinitionId",
                schema: "kenuser",
                table: "WorkflowStepInstances",
                column: "StepDefinitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowApprovalRoles",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "WorkflowConditions",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "WorkflowStepInstances",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "WorkflowInstances",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "WorkflowStepDefinitions",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "WorkflowDefinitions",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RequestApprovals",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 550, DateTimeKind.Local).AddTicks(4215),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 377, DateTimeKind.Local).AddTicks(6813));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 573, DateTimeKind.Local).AddTicks(4848),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 421, DateTimeKind.Local).AddTicks(8460));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 572, DateTimeKind.Local).AddTicks(4608),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 420, DateTimeKind.Local).AddTicks(9341));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(2848),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(6123));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 575, DateTimeKind.Local).AddTicks(2679),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 423, DateTimeKind.Local).AddTicks(8779));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 578, DateTimeKind.Local).AddTicks(1232),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 428, DateTimeKind.Local).AddTicks(4709));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(937),
                comment: "Part of composite unique key index",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1851),
                oldComment: "Part of composite unique key index");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(539),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1191));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(564),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(6139));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 7, 9, 14, 58, 572, DateTimeKind.Utc).AddTicks(142),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 4, 12, 12, 30, 48, 420, DateTimeKind.Utc).AddTicks(6769));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(5968),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 427, DateTimeKind.Local).AddTicks(3490));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(3329),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(9269));
        }
    }
}
