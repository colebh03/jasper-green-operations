/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      JasperGreenDbContext.cs

PURPOSE:      Defines the Entity Framework Core database context for the
              JasperGreen system, managing entity sets and configuring
              relationships and seed data.

INPUT:        Configuration options provided via dependency injection,
              including connection strings and database provider settings.

PROCESS:      Maps domain models to database tables, establishes entity
              relationships, and seeds initial data using Fluent API.

OUTPUT:       A configured DbContext used to query and persist application
              data to the underlying database.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace JasperGreen.Models
{
    public class JasperGreenDbContext : IdentityDbContext<User>
    {        

        public JasperGreenDbContext(DbContextOptions<JasperGreenDbContext> options)
            : base(options)
        { }

        //Order may need to change (remove this parenthesis once it has been checked)
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Crew> Crews { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;


        //Seed data randomized using generative AI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //CUSTOMER TABLE
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Cust_ID = 1, Cust_Name = "Jordan Spieth", Cust_Billing_Address = "101 Fairway Dr", Cust_Billing_City = "Dallas", Cust_Billing_State = "TX", Cust_Billing_Zip = "75201", Cust_Phone = "2145551001", Cust_Email = "jspieth@email.com" },
                new Customer { Cust_ID = 2, Cust_Name = "Scottie Scheffler", Cust_Billing_Address = "202 Masters Ln", Cust_Billing_City = "Dallas", Cust_Billing_State = "TX", Cust_Billing_Zip = "75202", Cust_Phone = "2145551002", Cust_Email = "sscheffler@email.com" },
                new Customer { Cust_ID = 3, Cust_Name = "Will Zalatoris", Cust_Billing_Address = "303 Augusta Way", Cust_Billing_City = "Plano", Cust_Billing_State = "TX", Cust_Billing_Zip = "75023", Cust_Phone = "9725551003", Cust_Email = "wzalatoris@email.com" },
                new Customer { Cust_ID = 4, Cust_Name = "Tony Finau", Cust_Billing_Address = "404 Eagle Bend", Cust_Billing_City = "Houston", Cust_Billing_State = "TX", Cust_Billing_Zip = "77002", Cust_Phone = "7135551004", Cust_Email = "tfinau@email.com" },
                new Customer { Cust_ID = 5, Cust_Name = "Bryson DeChambeau", Cust_Billing_Address = "505 Greenview Ct", Cust_Billing_City = "Austin", Cust_Billing_State = "TX", Cust_Billing_Zip = "73301", Cust_Phone = "5125551005", Cust_Email = "bdechambeau@email.com" },
                new Customer { Cust_ID = 6, Cust_Name = "Max Homa", Cust_Billing_Address = "606 Pinehurst Dr", Cust_Billing_City = "Frisco", Cust_Billing_State = "TX", Cust_Billing_Zip = "75034", Cust_Phone = "4695551006", Cust_Email = "mhoma@email.com" },
                new Customer { Cust_ID = 7, Cust_Name = "Collin Morikawa", Cust_Billing_Address = "707 Clubhouse Blvd", Cust_Billing_City = "Fort Worth", Cust_Billing_State = "TX", Cust_Billing_Zip = "76102", Cust_Phone = "8175551007", Cust_Email = "cmorikawa@email.com" },
                new Customer { Cust_ID = 8, Cust_Name = "Xander Schauffele", Cust_Billing_Address = "808 Pebble Beach Rd", Cust_Billing_City = "San Antonio", Cust_Billing_State = "TX", Cust_Billing_Zip = "78205", Cust_Phone = "2105551008", Cust_Email = "xschauffele@email.com" },
                new Customer { Cust_ID = 9, Cust_Name = "Justin Thomas", Cust_Billing_Address = "909 Open Championship Way", Cust_Billing_City = "Bryan", Cust_Billing_State = "TX", Cust_Billing_Zip = "77802", Cust_Phone = "9795551009", Cust_Email = "jthomas@email.com" },
                new Customer { Cust_ID = 10, Cust_Name = "Rickie Fowler", Cust_Billing_Address = "1001 Players Club Dr", Cust_Billing_City = "College Station", Cust_Billing_State = "TX", Cust_Billing_Zip = "77840", Cust_Phone = "9795551010", Cust_Email = "rfowler@email.com" }
            );

            //EMPLOYEE TABLE
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Emp_ID = 1, Emp_First_Name = "Cole", Emp_Last_Name = "Howell", Emp_SSN = "123456789", Emp_Job_Title = "Owner", Emp_Hire_Date = new DateOnly(2019, 3, 15), Emp_Hourly_Rate = 26.50M },
                new Employee { Emp_ID = 2, Emp_First_Name = "Chris", Emp_Last_Name = "Lopez", Emp_SSN = "987654321", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2022, 5, 10), Emp_Hourly_Rate = 18.75M },
                new Employee { Emp_ID = 3, Emp_First_Name = "Aaron", Emp_Last_Name = "White", Emp_SSN = "111223333", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2023, 2, 20), Emp_Hourly_Rate = 19.25M },
                new Employee { Emp_ID = 4, Emp_First_Name = "Brian", Emp_Last_Name = "Hall", Emp_SSN = "222334444", Emp_Job_Title = "Foreman", Emp_Hire_Date = new DateOnly(2020, 8, 12), Emp_Hourly_Rate = 27.00M },
                new Employee { Emp_ID = 5, Emp_First_Name = "Kevin", Emp_Last_Name = "Young", Emp_SSN = "333445555", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2021, 11, 5), Emp_Hourly_Rate = 18.50M },
                new Employee { Emp_ID = 6, Emp_First_Name = "Daniel", Emp_Last_Name = "King", Emp_SSN = "444556666", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2022, 7, 18), Emp_Hourly_Rate = 19.00M },
                new Employee { Emp_ID = 7, Emp_First_Name = "Jason", Emp_Last_Name = "Scott", Emp_SSN = "555667777", Emp_Job_Title = "Foreman", Emp_Hire_Date = new DateOnly(2019, 4, 22), Emp_Hourly_Rate = 28.25M },
                new Employee { Emp_ID = 8, Emp_First_Name = "Mark", Emp_Last_Name = "Green", Emp_SSN = "666778888", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2023, 1, 9), Emp_Hourly_Rate = 18.90M },
                new Employee { Emp_ID = 9, Emp_First_Name = "Ryan", Emp_Last_Name = "Baker", Emp_SSN = "777889999", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2020, 10, 14), Emp_Hourly_Rate = 19.10M },
                new Employee { Emp_ID = 10, Emp_First_Name = "Eric", Emp_Last_Name = "Adams", Emp_SSN = "888990000", Emp_Job_Title = "Crew", Emp_Hire_Date = new DateOnly(2021, 6, 30), Emp_Hourly_Rate = 18.65M }
            );

            //PROPERTY TABLE
            modelBuilder.Entity<Property>().HasData(
                new Property { Property_ID = 1, Cust_ID = 1, Property_Address = "101 Fairway Dr", Property_City = "Dallas", Property_State = "TX", Property_ZIP = "75201", Property_Service_Fee = 65.00M },
                new Property { Property_ID = 2, Cust_ID = 1, Property_Address = "102 Fairway Dr", Property_City = "Dallas", Property_State = "TX", Property_ZIP = "75201", Property_Service_Fee = 85.00M },
                new Property { Property_ID = 3, Cust_ID = 2, Property_Address = "202 Masters Ln", Property_City = "Dallas", Property_State = "TX", Property_ZIP = "75202", Property_Service_Fee = 95.00M },
                new Property { Property_ID = 4, Cust_ID = 2, Property_Address = "203 Masters Ln", Property_City = "Dallas", Property_State = "TX", Property_ZIP = "75202", Property_Service_Fee = 120.00M },
                new Property { Property_ID = 5, Cust_ID = 3, Property_Address = "303 Augusta Way", Property_City = "Plano", Property_State = "TX", Property_ZIP = "75023", Property_Service_Fee = 75.00M },
                new Property { Property_ID = 6, Cust_ID = 4, Property_Address = "404 Eagle Bend", Property_City = "Houston", Property_State = "TX", Property_ZIP = "77002", Property_Service_Fee = 80.00M },
                new Property { Property_ID = 7, Cust_ID = 5, Property_Address = "505 Greenview Ct", Property_City = "Austin", Property_State = "TX", Property_ZIP = "73301", Property_Service_Fee = 90.00M },
                new Property { Property_ID = 8, Cust_ID = 6, Property_Address = "606 Pinehurst Dr", Property_City = "Frisco", Property_State = "TX", Property_ZIP = "75034", Property_Service_Fee = 85.00M },
                new Property { Property_ID = 9, Cust_ID = 7, Property_Address = "707 Clubhouse Blvd", Property_City = "Fort Worth", Property_State = "TX", Property_ZIP = "76102", Property_Service_Fee = 100.00M },
                new Property { Property_ID = 10, Cust_ID = 8, Property_Address = "808 Pebble Beach Rd", Property_City = "San Antonio", Property_State = "TX", Property_ZIP = "78205", Property_Service_Fee = 95.00M },
                new Property { Property_ID = 11, Cust_ID = 9, Property_Address = "909 Open Championship Way", Property_City = "Bryan", Property_State = "TX", Property_ZIP = "77802", Property_Service_Fee = 70.00M },
                new Property { Property_ID = 12, Cust_ID = 10, Property_Address = "1001 Players Club Dr", Property_City = "College Station", Property_State = "TX", Property_ZIP = "77840", Property_Service_Fee = 75.00M }
            );
            
            //CREW BUSINESS RULES
            modelBuilder.Entity<Crew>()
                .HasOne(c => c.Foreman)
                .WithMany()
                .HasForeignKey(c => c.Crew_Foreman)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Crew>()
                .HasOne(c => c.CrewMember1)
                .WithMany()
                .HasForeignKey(c => c.Crew_Member_1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Crew>()
                .HasOne(c => c.CrewMember2)
                .WithMany()
                .HasForeignKey(c => c.Crew_Member_2)
                .OnDelete(DeleteBehavior.Restrict);

            //CREW TABLE
            modelBuilder.Entity<Crew>().HasData(
                new Crew { Crew_ID = 1, Crew_Foreman = 2, Crew_Member_1 = 5, Crew_Member_2 = 4 },
                new Crew { Crew_ID = 2, Crew_Foreman = 3, Crew_Member_1 = 6, Crew_Member_2 = 7 },
                new Crew { Crew_ID = 3, Crew_Foreman = 8, Crew_Member_1 = 9, Crew_Member_2 = 10 }
            );       
            
            //SERVICE BUSINESS RULES
            modelBuilder.Entity<Service>()
                .HasOne(ps => ps.Crew)
                .WithMany(c => c.Services)
                .HasForeignKey(ps => ps.Crew_ID);

            modelBuilder.Entity<Service>()
                .HasOne(ps => ps.Customer)
                .WithMany(c => c.Services)
                .HasForeignKey(ps => ps.Cust_ID);

            modelBuilder.Entity<Service>()
                .HasOne(ps => ps.Property)
                .WithMany(p => p.Services)
                .HasForeignKey(ps => ps.Property_ID)
                .OnDelete(DeleteBehavior.Restrict);

            //SERVICE TABLE
            modelBuilder.Entity<Service>().HasData(
                new Service { Service_ID = 1, Crew_ID = 1, Cust_ID = 1, Property_ID = 1, Service_Date = DateTime.Parse("2026-03-02"), Service_Fee = 65.00M },
                new Service { Service_ID = 2, Crew_ID = 1, Cust_ID = 1, Property_ID = 2, Service_Date = DateTime.Parse("2026-03-18"), Service_Fee = 85.00M },
                new Service { Service_ID = 3, Crew_ID = 1, Cust_ID = 1, Property_ID = 1, Service_Date = DateTime.Parse("2026-04-08"), Service_Fee = 65.00M },
                new Service { Service_ID = 4, Crew_ID = 1, Cust_ID = 1, Property_ID = 2, Service_Date = DateTime.Parse("2026-05-01"), Service_Fee = 85.00M },
                new Service { Service_ID = 5, Crew_ID = 1, Cust_ID = 2, Property_ID = 3, Service_Date = DateTime.Parse("2026-03-05"), Service_Fee = 95.00M },
                new Service { Service_ID = 6, Crew_ID = 1, Cust_ID = 2, Property_ID = 4, Service_Date = DateTime.Parse("2026-03-26"), Service_Fee = 120.00M },
                new Service { Service_ID = 7, Crew_ID = 1, Cust_ID = 2, Property_ID = 3, Service_Date = DateTime.Parse("2026-04-16"), Service_Fee = 95.00M },
                new Service { Service_ID = 8, Crew_ID = 1, Cust_ID = 2, Property_ID = 4, Service_Date = DateTime.Parse("2026-05-07"), Service_Fee = 120.00M },
                new Service { Service_ID = 9, Crew_ID = 2, Cust_ID = 3, Property_ID = 5, Service_Date = DateTime.Parse("2026-03-10"), Service_Fee = 75.00M },
                new Service { Service_ID = 10, Crew_ID = 2, Cust_ID = 3, Property_ID = 5, Service_Date = DateTime.Parse("2026-04-02"), Service_Fee = 75.00M },
                new Service { Service_ID = 11, Crew_ID = 2, Cust_ID = 3, Property_ID = 5, Service_Date = DateTime.Parse("2026-05-06"), Service_Fee = 75.00M },
                new Service { Service_ID = 12, Crew_ID = 2, Cust_ID = 4, Property_ID = 6, Service_Date = DateTime.Parse("2026-03-12"), Service_Fee = 80.00M },
                new Service { Service_ID = 13, Crew_ID = 2, Cust_ID = 4, Property_ID = 6, Service_Date = DateTime.Parse("2026-04-09"), Service_Fee = 80.00M },
                new Service { Service_ID = 14, Crew_ID = 2, Cust_ID = 4, Property_ID = 6, Service_Date = DateTime.Parse("2026-05-13"), Service_Fee = 80.00M },
                new Service { Service_ID = 15, Crew_ID = 2, Cust_ID = 5, Property_ID = 7, Service_Date = DateTime.Parse("2026-03-08"), Service_Fee = 90.00M },
                new Service { Service_ID = 16, Crew_ID = 2, Cust_ID = 5, Property_ID = 7, Service_Date = DateTime.Parse("2026-04-05"), Service_Fee = 90.00M },
                new Service { Service_ID = 17, Crew_ID = 2, Cust_ID = 5, Property_ID = 7, Service_Date = DateTime.Parse("2026-05-10"), Service_Fee = 90.00M },
                new Service { Service_ID = 18, Crew_ID = 3, Cust_ID = 6, Property_ID = 8, Service_Date = DateTime.Parse("2026-03-14"), Service_Fee = 85.00M },
                new Service { Service_ID = 19, Crew_ID = 3, Cust_ID = 6, Property_ID = 8, Service_Date = DateTime.Parse("2026-04-11"), Service_Fee = 85.00M },
                new Service { Service_ID = 20, Crew_ID = 3, Cust_ID = 6, Property_ID = 8, Service_Date = DateTime.Parse("2026-05-08"), Service_Fee = 85.00M },
                new Service { Service_ID = 21, Crew_ID = 3, Cust_ID = 7, Property_ID = 9, Service_Date = DateTime.Parse("2026-03-06"), Service_Fee = 100.00M },
                new Service { Service_ID = 22, Crew_ID = 3, Cust_ID = 7, Property_ID = 9, Service_Date = DateTime.Parse("2026-04-15"), Service_Fee = 100.00M },
                new Service { Service_ID = 23, Crew_ID = 3, Cust_ID = 7, Property_ID = 9, Service_Date = DateTime.Parse("2026-05-12"), Service_Fee = 100.00M },
                new Service { Service_ID = 24, Crew_ID = 3, Cust_ID = 8, Property_ID = 10, Service_Date = DateTime.Parse("2026-03-09"), Service_Fee = 95.00M },
                new Service { Service_ID = 25, Crew_ID = 3, Cust_ID = 8, Property_ID = 10, Service_Date = DateTime.Parse("2026-04-12"), Service_Fee = 95.00M },
                new Service { Service_ID = 26, Crew_ID = 3, Cust_ID = 8, Property_ID = 10, Service_Date = DateTime.Parse("2026-05-09"), Service_Fee = 95.00M },
                new Service { Service_ID = 27, Crew_ID = 1, Cust_ID = 9, Property_ID = 11, Service_Date = DateTime.Parse("2026-03-04"), Service_Fee = 70.00M },
                new Service { Service_ID = 28, Crew_ID = 1, Cust_ID = 9, Property_ID = 11, Service_Date = DateTime.Parse("2026-04-07"), Service_Fee = 70.00M },
                new Service { Service_ID = 29, Crew_ID = 1, Cust_ID = 9, Property_ID = 11, Service_Date = DateTime.Parse("2026-05-05"), Service_Fee = 70.00M },
                new Service { Service_ID = 30, Crew_ID = 2, Cust_ID = 10, Property_ID = 12, Service_Date = DateTime.Parse("2026-03-01"), Service_Fee = 75.00M },
                new Service { Service_ID = 31, Crew_ID = 2, Cust_ID = 10, Property_ID = 12, Service_Date = DateTime.Parse("2026-04-14"), Service_Fee = 75.00M },
                new Service { Service_ID = 32, Crew_ID = 2, Cust_ID = 10, Property_ID = 12, Service_Date = DateTime.Parse("2026-05-15"), Service_Fee = 75.00M }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment { Payment_ID = 1, Service_ID = 1, Payment_Amount = 65.00M, Payment_Date = DateTime.Parse("2026-03-02"), Payment_Method = "Card" },
                new Payment { Payment_ID = 2, Service_ID = 2, Payment_Amount = 85.00M, Payment_Date = DateTime.Parse("2026-03-18"), Payment_Method = "Check" },
                new Payment { Payment_ID = 3, Service_ID = 3, Payment_Amount = 65.00M, Payment_Date = DateTime.Parse("2026-04-08"), Payment_Method = "Card" },
                new Payment { Payment_ID = 4, Service_ID = 5, Payment_Amount = 95.00M, Payment_Date = DateTime.Parse("2026-03-05"), Payment_Method = "Card" },
                new Payment { Payment_ID = 5, Service_ID = 6, Payment_Amount = 120.00M, Payment_Date = DateTime.Parse("2026-03-26"), Payment_Method = "Check" },
                new Payment { Payment_ID = 6, Service_ID = 7, Payment_Amount = 95.00M, Payment_Date = DateTime.Parse("2026-04-16"), Payment_Method = "Card" },
                new Payment { Payment_ID = 7, Service_ID = 9, Payment_Amount = 75.00M, Payment_Date = DateTime.Parse("2026-03-10"), Payment_Method = "Cash" },
                new Payment { Payment_ID = 8, Service_ID = 10, Payment_Amount = 75.00M, Payment_Date = DateTime.Parse("2026-04-02"), Payment_Method = "Card" },
                new Payment { Payment_ID = 9, Service_ID = 12, Payment_Amount = 80.00M, Payment_Date = DateTime.Parse("2026-03-12"), Payment_Method = "Check" },
                new Payment { Payment_ID = 10, Service_ID = 13, Payment_Amount = 80.00M, Payment_Date = DateTime.Parse("2026-04-09"), Payment_Method = "Card" },
                new Payment { Payment_ID = 11, Service_ID = 15, Payment_Amount = 90.00M, Payment_Date = DateTime.Parse("2026-03-08"), Payment_Method = "Card" },
                new Payment { Payment_ID = 12, Service_ID = 16, Payment_Amount = 90.00M, Payment_Date = DateTime.Parse("2026-04-05"), Payment_Method = "Cash" },
                new Payment { Payment_ID = 13, Service_ID = 18, Payment_Amount = 85.00M, Payment_Date = DateTime.Parse("2026-03-14"), Payment_Method = "Check" },
                new Payment { Payment_ID = 14, Service_ID = 19, Payment_Amount = 85.00M, Payment_Date = DateTime.Parse("2026-04-11"), Payment_Method = "Card" },
                new Payment { Payment_ID = 15, Service_ID = 21, Payment_Amount = 100.00M, Payment_Date = DateTime.Parse("2026-03-06"), Payment_Method = "Card" },
                new Payment { Payment_ID = 16, Service_ID = 22, Payment_Amount = 100.00M, Payment_Date = DateTime.Parse("2026-04-15"), Payment_Method = "Check" },
                new Payment { Payment_ID = 17, Service_ID = 24, Payment_Amount = 95.00M, Payment_Date = DateTime.Parse("2026-03-09"), Payment_Method = "Card" },
                new Payment { Payment_ID = 18, Service_ID = 25, Payment_Amount = 95.00M, Payment_Date = DateTime.Parse("2026-04-12"), Payment_Method = "Cash" },
                new Payment { Payment_ID = 19, Service_ID = 27, Payment_Amount = 70.00M, Payment_Date = DateTime.Parse("2026-03-04"), Payment_Method = "Check" },
                new Payment { Payment_ID = 20, Service_ID = 28, Payment_Amount = 70.00M, Payment_Date = DateTime.Parse("2026-04-07"), Payment_Method = "Card" },
                new Payment { Payment_ID = 21, Service_ID = 30, Payment_Amount = 75.00M, Payment_Date = DateTime.Parse("2026-03-01"), Payment_Method = "Cash" },
                new Payment { Payment_ID = 22, Service_ID = 31, Payment_Amount = 75.00M, Payment_Date = DateTime.Parse("2026-04-14"), Payment_Method = "Check" }
            );
        }
    }
}
