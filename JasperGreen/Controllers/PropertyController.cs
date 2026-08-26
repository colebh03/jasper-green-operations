/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PropertyController.cs

PURPOSE:      Handles CRUD operations for Property entities and coordinates
              related Customer data using ASP.NET MVC and EF Core.

INPUT:        HTTP requests containing route parameters and form-bound
              Property objects.

PROCESS:      Retrieves, inserts, updates, and deletes Property records.
              Uses a ViewModel to supply related Customer data and state
              selections for UI rendering.

OUTPUT:       IActionResult responses that render Razor views or redirect
              to controller actions.

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
    public class PropertyController : Controller
    {
        private JasperGreenDbContext Context { get; set; }

        public PropertyController(JasperGreenDbContext ctx) => Context = ctx;

        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST PROPERTY
          ===================================================================== */

        /// <summary>
        /// Handles requests to the /properties URL and displays the Property/List page (attribute routing)
        /// </summary>
        /// <returns>
        /// A ViewResult that renders the Property/List view.
        /// </returns>
        [Route("properties/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, string sortColumn = "property", string sortDirection = "asc")
        {
            // -----------------------------------------------------------------
            // Build base query with eager loading:
            // - Customer: who owns property          
            // IQueryable allows deferred execution until ToList() is called
            // -----------------------------------------------------------------
            IQueryable<Property> query = Context.Properties
                .Include(p => p.Customer);

            // Apply filters based on which button was used

            // Filter by Customer
            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Cust_ID == id);
            }

            //Dynamic Filtering Code
            bool isAsc = sortDirection == "asc";

            query = sortColumn switch
            {
            "property" => isAsc
                 ? query.OrderBy(p => p.Property_Address + " " + p.Property_City)
    :            query.OrderByDescending(p => p.Property_Address + " " + p.Property_City),
            
            "customer" => isAsc
                ? query.OrderBy(p => p.Customer.Cust_Name)
                : query.OrderByDescending(p => p.Customer.Cust_Name),

            "fee" => isAsc
                ? query.OrderBy(p => p.Property_Service_Fee)
                : query.OrderByDescending(p => p.Property_Service_Fee),

            // Default Fallback
            _ => isAsc
                ? query.OrderBy(p => p.Property_Address + " " + p.Property_City)
                : query.OrderByDescending(p => p.Property_Address + " " + p.Property_City),
            };

            // Current Filtering String Creator
            string currentFilterText = "";

            if (filter == "customer" && id != null)
            {
                var customer = Context.Customers.Find(id);

                currentFilterText =
                    $"Customer Filter: {customer?.Cust_Name}";
            }



            //Create ViewModel
            var vm = new PropertyListViewModel
            {
                Properties = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                Filter = filter,
                Id = id,
                CurrentFilterText = currentFilterText,
            };

            return View(vm);
        }

        /* =====================================================================
         FILTER BY CUSTOMER
         ===================================================================== */

        /// <summary>
        /// Displays form to select a customer for filtering properties.
        /// </summary>
        /// <returns>View with list of customers</returns>
        [HttpGet]
        [Route("properties/getcustomer")]
        public IActionResult GetCustomer()
        {
            // Populate dropdown list of customers
            var vm = new CustomerFilterViewModel
            {
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList()
            };

            return View(vm);
        }

        /// <summary>
        /// Processes selected customer filter and redirects to filtered list.
        /// </summary>
        /// <param name="vm">ViewModel containing selected Customer ID</param>
        /// <returns>Redirect to filtered List view</returns>
        [HttpPost]
        [Route("properties/getcustomer")]
        public IActionResult GetCustomer(CustomerFilterViewModel vm)
        {
            // Validation ensures a selection was made
            if (!ModelState.IsValid)
            {
                // Rebuild dropdown list since HTTP is stateless
                vm.Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList();

                return View(vm);
            }

            // Redirect applies filter to List action
            return RedirectToAction("List", new { filter = "customer", id = vm.Cust_ID });
        }


        /* =====================================================================
           ADD PROPERTY (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for creating a new property.
        /// </summary>
        /// <returns>View initialized with empty PropertyViewModel.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            var vm = new PropertyViewModel
            {
                Property = new Property(),
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),
                States = StateHelper.GetStates(), // CHATGPT
                Action = "Add"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           EDIT PROPERTY (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for editing an existing property.
        /// </summary>
        /// <param name="id">Primary key of the property.</param>
        /// <returns>View populated with property and related data.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //var property = Context.Properties.Find(id);

            var vm = new PropertyViewModel
            {
                Property = Context.Properties.Find(id),
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),
                States = StateHelper.GetStates(), // CHATGPT
                Action = "Edit"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           SAVE PROPERTY (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Processes form submission to create or update a property.
        /// </summary>
        /// <param name="property">Property entity bound from form input.</param>
        /// <returns>
        /// Redirects to List on success; otherwise returns form with validation errors.
        /// </returns>
        [HttpPost]
        public IActionResult Save(Property property)
        {
            // -----------------------------------------------------------------
            // Two Boolean "flags" used to clarify whether the operation is an
            // Add (INSERT) or an Edit (UPDATE).
            // - isPropertyAdd: true if a new record is being created.
            // - isPropertyEdit: true if an existing record is being updated.
            // Only one should remain true after the checks below.
            // -------
            bool isPropertyAdd = true;
            bool isPropertyEdit = true;

            // Determine whether this is an Add or Edit for the view if we must re-display.
            if (property.Property_ID == 0)
            {
                // PropertyID == 0 means this record does not exist yet in the DB.
                // Therefore, this is an Add operation.
                //ViewBag.Action = "Add";
                isPropertyEdit = false;  // Not an edit in this case.
            }

            else
            {
                // A non-zero PropertyID means this record already exists in the DB.
                // Therefore, this is an Edit operation.
                //ViewBag.Action = "Edit";
                isPropertyAdd = false;   // Not an add in this case.
            }

            if (ModelState.IsValid)
            {
                // ================================================================
                // MODEL VALIDATION PASSED
                // ------------------------------------------------
                // We can safely attempt to persist the record to the database.
                // ================================================================

                if (isPropertyAdd)
                {
                    // EF Core Add(): Marks entity state as "Added"
                    // so that SaveChanges() will insert a new row.
                    Context.Properties.Add(property);
                }

                if (isPropertyEdit)
                {
                    // EF Core Update(): Marks entity state as "Modified"
                    // so that SaveChanges() will generate an UPDATE SQL.
                    Context.Properties.Update(property);
                }                

                Context.SaveChanges();
                return RedirectToAction("List");
            
            }

            else
            {
                var vm = new PropertyViewModel
                {
                    Property = property,
                    Customers = Context.Customers
                        .OrderBy(c => c.Cust_Name)
                        .ToList(),
                    States = StateHelper.GetStates(), // CHATGPT
                    Action = isPropertyAdd ? "Add" : "Edit" //determines if we are in add or edit, defines action
                };

                return View("AddEdit", vm);
            }
        }

        /* =====================================================================
           DELETE PROPERTY (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays confirmation page for deleting a property.
        /// </summary>
        /// <param name="id">Primary key of the property to delete.</param>
        /// <returns>View showing property and associated customer details.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            //var property = Context.Properties
            //    .Include(p => p.Customer)
            //    .FirstOrDefault(p => p.Property_ID == id);

            //if (property == null)
            //{
            //    return NotFound();
            //}

            //return View(property);

            // -----------------------------------------------------------------
            // Retrieve the Incident along with its related Customer and Product
            // using EF Core Include() for eager loading.
            // - FirstOrDefault() returns the matching Incident if found.
            // - If no Incident exists with the given id, null is returned.
            // - Find() is NOT used here because Find() does not support Include()
            //   and would not load related navigation properties.
            // -----------------------------------------------------------------
            var property = Context.Properties
                        .Include(p => p.Customer)
                        .FirstOrDefault(p => p.Property_ID == id);

            // Pass the incident object (or null) to the Delete view.
            // The view will prompt the user to confirm deletion.
            return View(property);
        }

        /* =====================================================================
           DELETE PROPERTY (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Deletes a property after confirmation.
        /// </summary>
        /// <param name="property">Property entity to remove.</param>
        /// <returns>Redirects to List after deletion.</returns>
        [HttpPost]
        public IActionResult Delete(Property property)
        {           
            var hasServices = Context.Services
                .Any(ps => ps.Property_ID == property.Property_ID);

            //custom message for data validation rules
            if (hasServices)
            {
                TempData["Error"] =
                    "Cannot delete property assigned to a service event. Delete those service events first.";

                return RedirectToAction("List");
            }

            Context.Properties.Remove(property);

                // Commit the DELETE operation to the database.
            Context.SaveChanges();

            // Redirect back to the List action so the user sees the
            // updated table of incidents without the deleted record.
            return RedirectToAction("List");
        }
    }
}
