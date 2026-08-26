/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      EmployeeController.cs

PURPOSE:      Provides CRUD operations for Employee entities within the
              JasperGreen system using ASP.NET MVC and EF Core.

INPUT:        HTTP requests containing route parameters and form-bound
              Employee objects.

PROCESS:      Retrieves, inserts, updates, and deletes Employee records
              through Entity Framework Core. Uses ViewBag for simple UI
              state management.

OUTPUT:       IActionResult responses that render views or redirect to
              controller actions.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using Microsoft.AspNetCore.Authorization;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private JasperGreenDbContext Context { get; set; }

        public EmployeeController(JasperGreenDbContext ctx) => Context = ctx;

        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST EMPLOYEES
          ===================================================================== */

        /// <summary>
        /// Handles requests to the /employees URL and displays the Employee/List page (attribute routing)
        /// </summary>
        /// <returns>
        /// A ViewResult that renders the Employee/List view.
        /// </returns>
        [Route("employees")]
        public IActionResult List(string sortColumn = "name", string sortDirection = "asc")
        {
            IQueryable<Employee> query = Context.Employees;            

            //Dynamic Filtering Code
            bool isAsc = sortDirection == "asc";

            query = sortColumn switch
            {
                "name" => isAsc
                ? query.OrderBy(p => p.Emp_First_Name + " " + p.Emp_Last_Name)
    :           query.OrderByDescending(p => p.Emp_First_Name + " " + p.Emp_Last_Name),
                "jobtitle" => isAsc
                ? query.OrderBy(p => p.Emp_Job_Title)
                    : query.OrderByDescending(p => p.Emp_Job_Title),
                "hiredate" => isAsc
                ? query.OrderBy(p => p.Emp_Hire_Date)
                    : query.OrderByDescending(p => p.Emp_Hire_Date),

                // Default Fallback
                _ => isAsc
                    ? query.OrderBy(p => p.Emp_Full_Name)
                    : query.OrderByDescending(p => p.Emp_Full_Name)
            };

            //Create Viewmodel
            var vm = new EmployeeListViewModel
            {
                Employees = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }

        /* =====================================================================
           ADD EMPLOYEE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for creating a new employee.
        /// </summary>
        /// <returns>View initialized with empty Employee model.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add"; // Used by the view to render "Add" labels/titles.

            return View("AddEdit", new Employee());
        }

        /* =====================================================================
           EDIT EMPLOYEE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for editing an existing employee.
        /// </summary>
        /// <param name="id">Primary key of the employee.</param>
        /// <returns>View populated with employee data.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";  // Used by the view for headings/buttons

            var employee = Context.Employees.Find(id);  // retrieve employee            

            return View("AddEdit", employee);
        }

        /* =====================================================================
           SAVE EMPLOYEE (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Processes form submission to create or update an employee.
        /// </summary>
        /// <param name="employee">Employee entity bound from form input.</param>
        /// <returns>
        /// Redirects to List on success; otherwise returns form with validation errors.
        /// </returns>
        [HttpPost]
        public IActionResult Save(Employee employee)
        {
            // -----------------------------------------------------------------
            // Two Boolean "flags" used to clarify whether the operation is an
            // Add (INSERT) or an Edit (UPDATE).
            // - isCustomerAdd: true if a new record is being created.
            // - isCustomerEdit: true if an existing record is being updated.
            // Only one should remain true after the checks below.
            // -------
            bool isEmployeeAdd = true;
            bool isEmployeeEdit = true;

            // Determine whether this is an Add or Edit for the view if we must re-display.
            if (employee.Emp_ID == 0)
            {
                // Emp_ID == 0 means this record does not exist yet in the DB.
                // Therefore, this is an Add operation.
                ViewBag.Action = "Add";
                isEmployeeEdit = false;  // Not an edit in this case.
            }

            else
            {
                // A non-zero Emp_ID means this record already exists in the DB.
                // Therefore, this is an Edit operation.
                ViewBag.Action = "Edit";
                isEmployeeAdd = false;   // Not an add in this case.
            }

            if (ModelState.IsValid)
            {
                // ================================================================
                // MODEL VALIDATION PASSED
                // ------------------------------------------------
                // We can safely attempt to persist the record to the database.
                // ================================================================

                if (isEmployeeAdd)
                {
                    // EF Core Add(): Marks entity state as "Added"
                    // so that SaveChanges() will insert a new row.
                    Context.Employees.Add(employee);
                }
                if (isEmployeeEdit)
                {
                    // EF Core Update(): Marks entity state as "Modified"
                    // so that SaveChanges() will generate an UPDATE SQL.
                    Context.Employees.Update(employee);
                }

                // Commit the changes (INSERT or UPDATE) to the database.
                Context.SaveChanges();

                // Redirect to List (Post/Redirect/Get pattern) to prevent
                // duplicate submissions if the user refreshes the page.
                return RedirectToAction("List");
            }


            else
            {               
                return View("AddEdit", employee);
            }
        }

        /* =====================================================================
           DELETE EMPLOYEE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays confirmation page for deleting an employee.
        /// </summary>
        /// <param name="id">Primary key of the employee to delete.</param>
        /// <returns>View showing selected employee details.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // -----------------------------------------------------------------
            // Use EF Core Find() to retrieve a Employee by its primary key.
            // - If the id matches a row in the Employees table, a Employee
            //   object is returned.
            // - If no match is found, null is returned.
            // -----------------------------------------------------------------
            var employee = Context.Employees.Find(id); // retrieve employee from database

            // Pass the customer object (or null) to the Delete view.
            // The view will prompt the user to confirm deletion.
            return View(employee);
        }

        /* =====================================================================
           DELETE EMPLOYEE (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Deletes an employee after confirmation.
        /// </summary>
        /// <param name="employee">Employee entity to remove.</param>
        /// <returns>Redirects to List after deletion.</returns>
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            //Makes sure user isn't deleting an employee that is assigned to a crew:
            var isInCrew = Context.Crews.Any(c =>
                c.Crew_Foreman == employee.Emp_ID ||
                c.Crew_Member_1 == employee.Emp_ID ||
                c.Crew_Member_2 == employee.Emp_ID);

            if (isInCrew)
            {
                TempData["Error"] =
                     "Cannot delete employee assigned to a crew. Delete those crew assignments first.";              
                return RedirectToAction("List");
            }


            // -----------------------------------------------------------------
            // EF Core Remove(): marks the entity state as "Deleted."
            // On SaveChanges(), EF Core will generate and execute a DELETE SQL
            // statement against the Employees table using the Emp_ID.
            // -----------------------------------------------------------------
            Context.Employees.Remove(employee);

            // Commit the DELETE operation to the database.
            Context.SaveChanges();

            // Redirect back to the List action so the user sees the
            // updated table of employees without the deleted record.
            return RedirectToAction("List");
        }
    }
}
