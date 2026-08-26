/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      CrewController.cs

PURPOSE:      Implements CRUD operations for Crew entities within the
              JasperGreen system using ASP.NET MVC and Entity Framework Core.

INPUT:        HTTP requests (GET/POST) containing route data, query parameters,
              and form-bound Crew objects.

PROCESS:      Retrieves, creates, updates, and deletes Crew records via EF Core.
              Utilizes ViewModels to transfer structured data to views.

OUTPUT:       Returns IActionResult responses, typically rendering Razor views
              or redirecting to appropriate controller actions.

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
    public class CrewController : Controller
    {
        // Centralized EF Core DbContext for database interaction.
        private JasperGreenDbContext Context { get; set; }

        // Constructor injection ensures DbContext is provided by DI container.
        public CrewController(JasperGreenDbContext ctx) => Context = ctx;

        // Default route redirects to the main listing endpoint.
        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST CREW
          ===================================================================== */

        /// <summary>
        /// Handles requests to the /crews URL and displays the Crew/List page (attribute routing)
        /// </summary>
        /// <returns>
        /// A ViewResult that renders the Crew/List view.
        /// </returns>
        [Route("crews")]
        public IActionResult List(string sortColumn = "foreman", string sortDirection = "asc")
        {
            IQueryable<Crew> query = Context.Crews
                .Include(c => c.Foreman)
                .Include(c => c.CrewMember1)
                .Include(c => c.CrewMember2);

            //Dynamic Filtering Code
            bool isAsc = sortDirection == "asc";

            query = sortColumn switch
            {
                "foreman" => isAsc
                    ? query.OrderBy(c => c.Foreman.Emp_First_Name)
                           .ThenBy(c => c.Foreman.Emp_Last_Name)
                    : query.OrderByDescending(c => c.Foreman.Emp_First_Name)
                           .ThenByDescending(c => c.Foreman.Emp_Last_Name),

                "member1" => isAsc
                    ? query.OrderBy(c => c.CrewMember1.Emp_First_Name)
                           .ThenBy(c => c.CrewMember1.Emp_Last_Name)
                    : query.OrderByDescending(c => c.CrewMember1.Emp_First_Name)
                           .ThenByDescending(c => c.CrewMember1.Emp_Last_Name),

                "member2" => isAsc
                    ? query.OrderBy(c => c.CrewMember2.Emp_First_Name)
                           .ThenBy(c => c.CrewMember2.Emp_Last_Name)
                    : query.OrderByDescending(c => c.CrewMember2.Emp_First_Name)
                           .ThenByDescending(c => c.CrewMember2.Emp_Last_Name),

                // Default Fallback
                _ => isAsc
                    ? query.OrderBy(c => c.Foreman.Emp_First_Name)
                           .ThenBy(c => c.Foreman.Emp_Last_Name)
                    : query.OrderByDescending(c => c.Foreman.Emp_First_Name)
                           .ThenByDescending(c => c.Foreman.Emp_Last_Name)
            };

            //Create Viewmodel
            var vm = new CrewListViewModel
            {
                Crews = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }

        /* =====================================================================
           ADD CREW (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for creating a new crew.
        /// </summary>
        /// <returns>View containing an empty CrewViewModel for input.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            // ViewModel encapsulates both entity and supporting lookup data.
            // This avoids overloading the view with multiple ViewBag dependencies.
            var vm = new CrewViewModel
            {
                Crew = new Crew(), // Empty instance for model binding in the form.
                Employees = Context.Employees
                    .OrderBy(c => c.Emp_First_Name)
                    .ToList(),
                Action = "Add"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           EDIT CREW (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for editing an existing crew.
        /// </summary>
        /// <param name="id">Primary key of the crew to edit.</param>
        /// <returns>View populated with the selected crew and employee list.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find() leverages primary key lookup and EF tracking cache if available.
            var crew = Context.Crews.Find(id);

            var vm = new CrewViewModel
            {
                Crew = crew, // Existing entity pre-populates the form fields.
                Employees = Context.Employees
                    .OrderBy(c => c.Emp_First_Name)
                    .ToList(),
                Action = "Edit"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           SAVE CREW (HTTP POST)
           ===================================================================== */
       
        /// <summary>
        /// Processes form submission for creating or updating a crew.
        /// </summary>
        /// <param name="crew">Crew entity bound from form data.</param>
        /// <returns>
        /// Redirects to List on success; otherwise re-renders the Add/Edit view with validation errors.
        /// </returns>
        [HttpPost]
        public IActionResult Save(Crew crew)
        {
            // -----------------------------------------------------------------
            // Two Boolean "flags" used to clarify whether the operation is an
            // Add (INSERT) or an Edit (UPDATE).
            // - isPropertyAdd: true if a new record is being created.
            // - isPropertyEdit: true if an existing record is being updated.
            // Only one should remain true after the checks below.
            // -------
            bool isCrewAdd = true;
            bool isCrewEdit = true;

            // Determine whether this is an Add or Edit for the view if we must re-display.
            if (crew.Crew_ID == 0)
            {
                // Crew_ID == 0 means this record does not exist yet in the DB.
                // Therefore, this is an Add operation.
                isCrewEdit = false;  // Not an edit in this case.
            }

            else
            {
                // A non-zero Crew_ID means this record already exists in the DB.
                // Therefore, this is an Edit operation.
                isCrewAdd = false;   // Not an add in this case.
            }

            if (ModelState.IsValid)
            {
                // ================================================================
                // MODEL VALIDATION PASSED
                // ------------------------------------------------
                // We can safely attempt to persist the record to the database.
                // ================================================================

                if (isCrewAdd)
                {
                    // EF Core Add(): Marks entity state as "Added"
                    // so that SaveChanges() will insert a new row.
                    Context.Crews.Add(crew);
                }
                if (isCrewEdit)
                {
                    // EF Core Update(): Marks entity state as "Modified"
                    // so that SaveChanges() will generate an UPDATE SQL.
                    Context.Crews.Update(crew);
                }

                // Commit the changes (INSERT or UPDATE) to the database.
                Context.SaveChanges();

                // Redirect to List (Post/Redirect/Get pattern) to prevent
                // duplicate submissions if the user refreshes the page.
                return RedirectToAction("List");

            }

            else
            {
                // Rehydrate ViewModel since HTTP is stateless and view requires lookup data again.
                var vm = new CrewViewModel
                {
                    Crew = crew,
                    Employees = Context.Employees
                        .OrderBy(c => c.Emp_First_Name)
                        .ToList(),
                    Action = isCrewAdd ? "Add" : "Edit" //determines if we are in add or edit, defines action
                };

                return View("AddEdit", vm);
            }
        }

        /* =====================================================================
           DELETE CREW (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays a confirmation view for deleting a crew.
        /// </summary>
        /// <param name="id">Primary key of the crew to delete.</param>
        /// <returns>View showing crew details for deletion confirmation.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Eager loading ensures related employee data is available for confirmation UI.
            var crew = Context.Crews
                 .Include(c => c.Foreman)
                .Include(c => c.CrewMember1)
                .Include(c => c.CrewMember2)
                .FirstOrDefault(c => c.Crew_ID == id);

            // View is responsible for null handling and confirmation logic.
            return View(crew);
        }

        /* =====================================================================
           DELETE CREW (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Executes deletion of a crew after confirmation.
        /// </summary>
        /// <param name="crew">Crew entity to delete.</param>
        /// <returns>Redirects to List after successful deletion.</returns>
        [HttpPost]
        public IActionResult Delete(Crew crew)
        {
            var isInService = Context.Services.Any(ps =>
                ps.Crew_ID == crew.Crew_ID);

            //custom error message, keeping data integrity rules
            if (isInService)
            {
                TempData["Error"] =
                    "Cannot delete crew assigned to a service event. Delete those service events first.";

                return RedirectToAction("List");
            }

            // Direct removal assumes entity is correctly bound and tracked or attachable.
            Context.Crews.Remove(crew);

            // Commit the DELETE operation to the database.
            Context.SaveChanges();

            // Redirect back to the List action so the user sees the
            // Updated table of incidents without the deleted record.
            return RedirectToAction("List");
        }

    }
}
