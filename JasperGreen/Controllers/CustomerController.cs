/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      CustomerController.cs

PURPOSE:      Manages CRUD operations for Customer entities within the
              JasperGreen system using ASP.NET MVC and EF Core.

INPUT:        HTTP requests containing route parameters, query values,
              and form-bound Customer objects.

PROCESS:      Retrieves, inserts, updates, and deletes Customer records
              through Entity Framework Core. Uses ViewBag to pass
              auxiliary UI data such as state selections.

OUTPUT:       IActionResult responses that render Razor views or
              redirect to controller actions.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // Centralized EF Core DbContext for database interaction.
        private JasperGreenDbContext Context { get; set; }

        // Constructor injection ensures DbContext is provided by DI container.
        public CustomerController(JasperGreenDbContext ctx) => Context = ctx;

        // Default route redirects to the main listing endpoint.
        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST CUSTOMERS
          ===================================================================== */

        /// <summary>
        /// Handles requests to the /customers URL and displays the Customer/List page (attribute routing)
        /// </summary>
        /// <returns>
        /// A ViewResult that renders the Customer/List view.
        /// </returns>
        [Route("customers")]
        public IActionResult List(string sortColumn = "name", string sortDirection = "asc")
        {
            IQueryable<Customer> query = Context.Customers;

            //Dynamic Filtering Code
            bool isAsc = sortDirection == "asc";

            query = sortColumn switch
            {
                "name" => isAsc
                ? query.OrderBy(p => p.Cust_Name)
                    : query.OrderByDescending(p => p.Cust_Name),
                "city" => isAsc
                ? query.OrderBy(p => p.Cust_Billing_City)
                    : query.OrderByDescending(p => p.Cust_Billing_City),                

                // Default Fallback
                _ => isAsc
                    ? query.OrderBy(p => p.Cust_Name)
                    : query.OrderByDescending(p => p.Cust_Name)
            };

            //Create Viewmodel
            var vm = new CustomerListViewModel
            {
                Customers = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }

        /* =====================================================================
           ADD CUSTOMER (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for creating a new customer.
        /// </summary>
        /// <returns>View initialized with empty Customer model and state list.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add"; // Used by the view to render "Add" labels/titles.

            // Used to create state dropdown box that lists state full names but underlying ID is the state code
            ViewBag.States = StateHelper.GetStates();

            return View("AddEdit", new Customer());
        }

        /* =====================================================================
           EDIT CUSTOMER (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays the form for editing an existing customer.
        /// </summary>
        /// <param name="id">Primary key of the customer.</param>
        /// <returns>View populated with customer data and state list.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";  // Used by the view for headings/buttons

            var customer = Context.Customers.Find(id);  // retrieve customer            

            ViewBag.States = StateHelper.GetStates(); //CHATGPT

            return View("AddEdit", customer);
        }

        /* =====================================================================
           SAVE CUSTOMER (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Processes form submission to create or update a customer.
        /// </summary>
        /// <param name="customer">Customer entity bound from form input.</param>
        /// <returns>
        /// Redirects to List on success; otherwise returns form with validation errors.
        /// </returns>
        [HttpPost]
        public IActionResult Save(Customer customer)
        {
            // -----------------------------------------------------------------
            // Two Boolean "flags" used to clarify whether the operation is an
            // Add (INSERT) or an Edit (UPDATE).
            // - isCustomerAdd: true if a new record is being created.
            // - isCustomerEdit: true if an existing record is being updated.
            // Only one should remain true after the checks below.
            // -------
            bool isCustomerAdd = true;
            bool isCustomerEdit = true;

            // Determine whether this is an Add or Edit for the view if we must re-display.
            if (customer.Cust_ID == 0)
            {
                // CustomerID == 0 means this record does not exist yet in the DB.
                // Therefore, this is an Add operation.
                ViewBag.Action = "Add";
                isCustomerEdit = false;  // Not an edit in this case.
            }

            else
            {
                // A non-zero CustomerID means this record already exists in the DB.
                // Therefore, this is an Edit operation.
                ViewBag.Action = "Edit";
                isCustomerAdd = false;   // Not an add in this case.
            }

            if (ModelState.IsValid)
            {
                // ================================================================
                // MODEL VALIDATION PASSED
                // ------------------------------------------------
                // We can safely attempt to persist the record to the database.
                // ================================================================

                if (isCustomerAdd)
                {
                    // EF Core Add(): Marks entity state as "Added"
                    // so that SaveChanges() will insert a new row.
                    Context.Customers.Add(customer);
                }
                if (isCustomerEdit)
                {
                    // EF Core Update(): Marks entity state as "Modified"
                    // so that SaveChanges() will generate an UPDATE SQL.
                    Context.Customers.Update(customer);
                }

                // Commit the changes (INSERT or UPDATE) to the database.
                Context.SaveChanges();

                //TempData message builders
                if (isCustomerAdd)
                {
                    TempData["message"] = $"{customer.Cust_Name} was added.";
                }
                else //implies it will be from edit, not add
                {
                    TempData["message"] = $"{customer.Cust_Name} was updated.";
                }



                return RedirectToAction("List");
            }


            else
            {
                ViewBag.States = StateHelper.GetStates();
                return View("AddEdit", customer);
            }
        }

        /* =====================================================================
           DELETE CUSTOMER (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays confirmation page for deleting a customer.
        /// </summary>
        /// <param name="id">Primary key of the customer to delete.</param>
        /// <returns>View showing selected customer details.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // -----------------------------------------------------------------
            // Use EF Core Find() to retrieve a Customer by its primary key.
            // - If the id matches a row in the Customers table, a Customer
            //   object is returned.
            // - If no match is found, null is returned.
            // -----------------------------------------------------------------
            var customer = Context.Customers.Find(id); // retrieve customer from database

            // Pass the customer object (or null) to the Delete view.
            // The view will prompt the user to confirm deletion.
            return View(customer);
        }

        /* =====================================================================
           DELETE CUSTOMER (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Deletes a customer after confirmation.
        /// </summary>
        /// <param name="customer">Customer entity to remove.</param>
        /// <returns>Redirects to List after deletion.</returns>
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            var isInProperty = Context.Properties.Any(p =>
                p.Cust_ID == customer.Cust_ID);

            var isInService = Context.Services.Any(ps =>
                ps.Cust_ID == customer.Cust_ID);

            //custom message for data validation rules
            if (isInProperty || isInService)
            {
                TempData["Error"] =
                    "Cannot delete customer with existing properties, service events, or payments. Delete those records first.";

                return RedirectToAction("List");
            }
            
            Context.Customers.Remove(customer);

            // Commit the DELETE operation to the database.
            Context.SaveChanges();

            // Redirect back to the List action so the user sees the
            // updated table of customers without the deleted record.
            return RedirectToAction("List");        
        }

    }
}
