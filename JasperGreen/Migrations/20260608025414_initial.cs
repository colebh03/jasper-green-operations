using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JasperGreen.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Cust_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cust_Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Cust_Billing_Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cust_Billing_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cust_Billing_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cust_Billing_Zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cust_Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cust_Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Cust_ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Emp_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emp_First_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Emp_Last_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Emp_SSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Job_Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Emp_Hire_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Emp_Hourly_Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Emp_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Property_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cust_ID = table.Column<int>(type: "int", nullable: false),
                    Property_Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Property_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Property_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Property_ZIP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Property_Service_Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Property_ID);
                    table.ForeignKey(
                        name: "FK_Properties_Customers_Cust_ID",
                        column: x => x.Cust_ID,
                        principalTable: "Customers",
                        principalColumn: "Cust_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Crew_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crew_Foreman = table.Column<int>(type: "int", nullable: false),
                    Crew_Member_1 = table.Column<int>(type: "int", nullable: false),
                    Crew_Member_2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Crew_ID);
                    table.ForeignKey(
                        name: "FK_Crews_Employees_Crew_Foreman",
                        column: x => x.Crew_Foreman,
                        principalTable: "Employees",
                        principalColumn: "Emp_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Crews_Employees_Crew_Member_1",
                        column: x => x.Crew_Member_1,
                        principalTable: "Employees",
                        principalColumn: "Emp_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Crews_Employees_Crew_Member_2",
                        column: x => x.Crew_Member_2,
                        principalTable: "Employees",
                        principalColumn: "Emp_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Service_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crew_ID = table.Column<int>(type: "int", nullable: false),
                    Cust_ID = table.Column<int>(type: "int", nullable: false),
                    Property_ID = table.Column<int>(type: "int", nullable: false),
                    Service_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Service_Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Service_ID);
                    table.ForeignKey(
                        name: "FK_Services_Crews_Crew_ID",
                        column: x => x.Crew_ID,
                        principalTable: "Crews",
                        principalColumn: "Crew_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Customers_Cust_ID",
                        column: x => x.Cust_ID,
                        principalTable: "Customers",
                        principalColumn: "Cust_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Properties_Property_ID",
                        column: x => x.Property_ID,
                        principalTable: "Properties",
                        principalColumn: "Property_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Payment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service_ID = table.Column<int>(type: "int", nullable: false),
                    Payment_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payment_Method = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Payment_ID);
                    table.ForeignKey(
                        name: "FK_Payments_Services_Service_ID",
                        column: x => x.Service_ID,
                        principalTable: "Services",
                        principalColumn: "Service_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Cust_ID", "Cust_Billing_Address", "Cust_Billing_City", "Cust_Billing_State", "Cust_Billing_Zip", "Cust_Email", "Cust_Name", "Cust_Phone" },
                values: new object[,]
                {
                    { 1, "101 Fairway Dr", "Dallas", "TX", "75201", "jspieth@email.com", "Jordan Spieth", "2145551001" },
                    { 2, "202 Masters Ln", "Dallas", "TX", "75202", "sscheffler@email.com", "Scottie Scheffler", "2145551002" },
                    { 3, "303 Augusta Way", "Plano", "TX", "75023", "wzalatoris@email.com", "Will Zalatoris", "9725551003" },
                    { 4, "404 Eagle Bend", "Houston", "TX", "77002", "tfinau@email.com", "Tony Finau", "7135551004" },
                    { 5, "505 Greenview Ct", "Austin", "TX", "73301", "bdechambeau@email.com", "Bryson DeChambeau", "5125551005" },
                    { 6, "606 Pinehurst Dr", "Frisco", "TX", "75034", "mhoma@email.com", "Max Homa", "4695551006" },
                    { 7, "707 Clubhouse Blvd", "Fort Worth", "TX", "76102", "cmorikawa@email.com", "Collin Morikawa", "8175551007" },
                    { 8, "808 Pebble Beach Rd", "San Antonio", "TX", "78205", "xschauffele@email.com", "Xander Schauffele", "2105551008" },
                    { 9, "909 Open Championship Way", "Bryan", "TX", "77802", "jthomas@email.com", "Justin Thomas", "9795551009" },
                    { 10, "1001 Players Club Dr", "College Station", "TX", "77840", "rfowler@email.com", "Rickie Fowler", "9795551010" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Emp_ID", "Emp_First_Name", "Emp_Hire_Date", "Emp_Hourly_Rate", "Emp_Job_Title", "Emp_Last_Name", "Emp_SSN" },
                values: new object[,]
                {
                    { 1, "Cole", new DateOnly(2019, 3, 15), 26.50m, "Owner", "Howell", "123456789" },
                    { 2, "Chris", new DateOnly(2022, 5, 10), 18.75m, "Crew", "Lopez", "987654321" },
                    { 3, "Aaron", new DateOnly(2023, 2, 20), 19.25m, "Crew", "White", "111223333" },
                    { 4, "Brian", new DateOnly(2020, 8, 12), 27.00m, "Foreman", "Hall", "222334444" },
                    { 5, "Kevin", new DateOnly(2021, 11, 5), 18.50m, "Crew", "Young", "333445555" },
                    { 6, "Daniel", new DateOnly(2022, 7, 18), 19.00m, "Crew", "King", "444556666" },
                    { 7, "Jason", new DateOnly(2019, 4, 22), 28.25m, "Foreman", "Scott", "555667777" },
                    { 8, "Mark", new DateOnly(2023, 1, 9), 18.90m, "Crew", "Green", "666778888" },
                    { 9, "Ryan", new DateOnly(2020, 10, 14), 19.10m, "Crew", "Baker", "777889999" },
                    { 10, "Eric", new DateOnly(2021, 6, 30), 18.65m, "Crew", "Adams", "888990000" }
                });

            migrationBuilder.InsertData(
                table: "Crews",
                columns: new[] { "Crew_ID", "Crew_Foreman", "Crew_Member_1", "Crew_Member_2" },
                values: new object[,]
                {
                    { 1, 2, 5, 4 },
                    { 2, 3, 6, 7 },
                    { 3, 8, 9, 10 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Property_ID", "Cust_ID", "Property_Address", "Property_City", "Property_Service_Fee", "Property_State", "Property_ZIP" },
                values: new object[,]
                {
                    { 1, 1, "101 Fairway Dr", "Dallas", 65.00m, "TX", "75201" },
                    { 2, 1, "102 Fairway Dr", "Dallas", 85.00m, "TX", "75201" },
                    { 3, 2, "202 Masters Ln", "Dallas", 95.00m, "TX", "75202" },
                    { 4, 2, "203 Masters Ln", "Dallas", 120.00m, "TX", "75202" },
                    { 5, 3, "303 Augusta Way", "Plano", 75.00m, "TX", "75023" },
                    { 6, 4, "404 Eagle Bend", "Houston", 80.00m, "TX", "77002" },
                    { 7, 5, "505 Greenview Ct", "Austin", 90.00m, "TX", "73301" },
                    { 8, 6, "606 Pinehurst Dr", "Frisco", 85.00m, "TX", "75034" },
                    { 9, 7, "707 Clubhouse Blvd", "Fort Worth", 100.00m, "TX", "76102" },
                    { 10, 8, "808 Pebble Beach Rd", "San Antonio", 95.00m, "TX", "78205" },
                    { 11, 9, "909 Open Championship Way", "Bryan", 70.00m, "TX", "77802" },
                    { 12, 10, "1001 Players Club Dr", "College Station", 75.00m, "TX", "77840" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Service_ID", "Crew_ID", "Cust_ID", "Property_ID", "Service_Date", "Service_Fee" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 65.00m },
                    { 2, 1, 1, 2, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m },
                    { 3, 1, 1, 1, new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 65.00m },
                    { 4, 1, 1, 2, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m },
                    { 5, 1, 2, 3, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.00m },
                    { 6, 1, 2, 4, new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 120.00m },
                    { 7, 1, 2, 3, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.00m },
                    { 8, 1, 2, 4, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 120.00m },
                    { 9, 2, 3, 5, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m },
                    { 10, 2, 3, 5, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m },
                    { 11, 2, 3, 5, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m },
                    { 12, 2, 4, 6, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 80.00m },
                    { 13, 2, 4, 6, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 80.00m },
                    { 14, 2, 4, 6, new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 80.00m },
                    { 15, 2, 5, 7, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 90.00m },
                    { 16, 2, 5, 7, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 90.00m },
                    { 17, 2, 5, 7, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 90.00m },
                    { 18, 3, 6, 8, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m },
                    { 19, 3, 6, 8, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m },
                    { 20, 3, 6, 8, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m },
                    { 21, 3, 7, 9, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 100.00m },
                    { 22, 3, 7, 9, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 100.00m },
                    { 23, 3, 7, 9, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 100.00m },
                    { 24, 3, 8, 10, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.00m },
                    { 25, 3, 8, 10, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.00m },
                    { 26, 3, 8, 10, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.00m },
                    { 27, 1, 9, 11, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.00m },
                    { 28, 1, 9, 11, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.00m },
                    { 29, 1, 9, 11, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.00m },
                    { 30, 2, 10, 12, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m },
                    { 31, 2, 10, 12, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m },
                    { 32, 2, 10, 12, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Payment_ID", "Payment_Amount", "Payment_Date", "Payment_Method", "Service_ID" },
                values: new object[,]
                {
                    { 1, 65.00m, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 1 },
                    { 2, 85.00m, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 2 },
                    { 3, 65.00m, new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 3 },
                    { 4, 95.00m, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 5 },
                    { 5, 120.00m, new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 6 },
                    { 6, 95.00m, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 7 },
                    { 7, 75.00m, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", 9 },
                    { 8, 75.00m, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 10 },
                    { 9, 80.00m, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 12 },
                    { 10, 80.00m, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 13 },
                    { 11, 90.00m, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 15 },
                    { 12, 90.00m, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", 16 },
                    { 13, 85.00m, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 18 },
                    { 14, 85.00m, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 19 },
                    { 15, 100.00m, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 21 },
                    { 16, 100.00m, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 22 },
                    { 17, 95.00m, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 24 },
                    { 18, 95.00m, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", 25 },
                    { 19, 70.00m, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 27 },
                    { 20, 70.00m, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card", 28 },
                    { 21, 75.00m, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", 30 },
                    { 22, 75.00m, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check", 31 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Crew_Foreman",
                table: "Crews",
                column: "Crew_Foreman");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Crew_Member_1",
                table: "Crews",
                column: "Crew_Member_1");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Crew_Member_2",
                table: "Crews",
                column: "Crew_Member_2");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Service_ID",
                table: "Payments",
                column: "Service_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Cust_ID",
                table: "Properties",
                column: "Cust_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Crew_ID",
                table: "Services",
                column: "Crew_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Cust_ID",
                table: "Services",
                column: "Cust_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Property_ID",
                table: "Services",
                column: "Property_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
