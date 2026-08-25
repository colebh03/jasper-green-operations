/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PaymentController.cs

PURPOSE:      
              This controller manages all CRUD operations for Payment entities
              within the JasperGreen application. It coordinates between the
              UI (Views), business logic, and the database via Entity Framework Core.

INPUT:        
              - HTTP GET/POST requests from user interactions (form submissions,
                navigation requests)
              - Payment objects bound from form data
              - Route parameters (e.g., Payment_ID)

PROCESS:      
              - Retrieves payment and related customer data from the database
              - Determines whether operations are Add (INSERT) or Edit (UPDATE)
              - Validates model state before committing changes
              - Uses Entity Framework Core to persist data
              - Prevents deletion when referential integrity would be violated

OUTPUT:       
              - Returns Views (List, AddEdit, Delete)
              - Redirects to appropriate actions after operations
              - Provides validation feedback or error messaging via ModelState/TempData

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
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private JasperGreenDbContext Context { get; set; }
        
        public PaymentController(JasperGreenDbContext ctx) => Context = ctx;
        
        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST PAYMENT
          ===================================================================== */

        /// <summary>
        /// Retrieves all payments along with their associated customer data.
        /// </summary>
        /// <returns>View containing list of payments</returns>
        /// <remarks>
        /// Include() ensures related Customer data is loaded in the same query.
        /// OrderBy() ensures consistent and readable display ordering.
        /// </remarks>
        [Route("payments/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, DateTime? startDate = null, DateTime? endDate = null, string sortColumn = "date", string sortDirection = "desc")
        {
            IQueryable<Payment> query = Context.Payments
                .Include(p => p.Service)
                    .ThenInclude(s => s.Customer)
                .Include(p => p.Service)
                    .ThenInclude(s => s.Property);

            // Apply filters based on which button was used

            // Filter by Date Range
            if (filter == "date" &&
                startDate != null &&
                endDate != null)
            {
                query = query.Where(p =>
                    p.Payment_Date >= startDate &&
                    p.Payment_Date < endDate.Value.AddDays(1));
            }

            // Filter by Customer
            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Service.Cust_ID == id);
            }

            // Default behavior:
            // Date, Amount, and ID naturally default to DESC
            // Customer and Method naturally default to ASC
            //Dynamic Filtering Code
            bool isAsc = sortColumn switch
            {
                "date" => sortDirection == "asc",
                "amount" => sortDirection == "asc",
                "paymentid" => sortDirection == "asc",

                "name" => sortDirection != "desc",
                "method" => sortDirection != "desc",

                _ => sortDirection == "asc"
            };

            query = sortColumn switch
            {
                "date" => isAsc
                    ? query.OrderBy(p => p.Payment_Date)
                    : query.OrderByDescending(p => p.Payment_Date),

                "name" => isAsc
                    ? query.OrderBy(p => p.Service.Customer.Cust_Name)
                    : query.OrderByDescending(p => p.Service.Customer.Cust_Name),

                "amount" => isAsc
                    ? query.OrderBy(p => p.Payment_Amount)
                    : query.OrderByDescending(p => p.Payment_Amount),

                "method" => isAsc
                    ? query.OrderBy(p => p.Payment_Method)
                    : query.OrderByDescending(p => p.Payment_Method),

                "paymentid" => isAsc
                    ? query.OrderBy(p => p.Payment_ID)
                    : query.OrderByDescending(p => p.Payment_ID),

                // Default fallback
                _ => query.OrderByDescending(p => p.Payment_Date)
            };

            // Current Filtering String Creator
            string currentFilterText = "";

            if (filter == "customer" && id != null)
            {
                var customer = Context.Customers.Find(id);

                currentFilterText =
                    $"Customer Filter: {customer?.Cust_Name}";
            }
            else if (filter == "date" &&
                     startDate != null &&
                     endDate != null)
            {
                currentFilterText =
                    $"Date Filter: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
            }

            // Create ViewModel
            var vm = new PaymentListViewModel
            {
                Payments = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                Filter = filter,
                Id = id,
                CurrentFilterText = currentFilterText
            };

            return View(vm);
        }

        /* =====================================================================
          FILTER PAYMENTS BY DATE
          ===================================================================== */

        /// <summary>
        /// Displays form to select a date range for filtering payments.
        /// </summary>
        /// <returns>View with date range inputs</returns>
        [HttpGet]
        [Route("payments/getdate")]
        public IActionResult GetDate()
        {
            var vm = new DateFilterViewModel();

            return View(vm);
        }

        /// <summary>
        /// Processes selected date range and redirects to filtered payment list.
        /// </summary>
        /// <param name="vm">
        /// ViewModel containing selected StartDate and EndDate
        /// </param>
        /// <returns>Redirect to filtered List view</returns>
        [HttpPost]
        [Route("payments/getdate")]
        public IActionResult GetDate(DateFilterViewModel vm)
        {
            // Validation ensures both dates were entered
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Redirect applies date filter to List action
            return RedirectToAction("List",
                new
                {
                    filter = "date",
                    startDate = vm.StartDate,
                    endDate = vm.EndDate
                });
        }


        /* =====================================================================
          FILTER PAYMENTS BY CUSTOMER
          ===================================================================== */

        /// <summary>
        /// Displays form to select a customer for filtering payments.
        /// </summary>
        /// <returns>View with list of customers</returns>
        [HttpGet]
        [Route("payments/getcustomer")]
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
        /// Processes selected customer filter and redirects to filtered payment list.
        /// </summary>
        /// <param name="vm">ViewModel containing selected Customer ID</param>
        /// <returns>Redirect to filtered List view</returns>
        [HttpPost]
        [Route("payments/getcustomer")]
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
            return RedirectToAction("List",
                new
                {
                    filter = "customer",
                    id = vm.Cust_ID
                });
        }


        /* =====================================================================
           ADD PAYMENT (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays form for creating a new payment.
        /// </summary>
        /// <returns>View with initialized ViewModel</returns>
        /// <remarks>
        /// ViewModel combines entity and supporting UI data.
        /// </remarks>
        [HttpGet]
        public IActionResult Add(int id)
        {
            // Retrieve associated service
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Payment)
                .FirstOrDefault(s => s.Service_ID == id);

            //Back-end protection, one service = one payment maximum
            if (service == null)
            {
                return RedirectToAction("List", "Service");
            }

            if (service.Payment != null)
            {
                return RedirectToAction("List", "Service");
            }

            // Create ViewModel
            var vm = new PaymentViewModel
            {
                Payment = new Payment
                {
                    Service_ID = id,
                    Payment_Amount = service.Service_Fee,
                    Payment_Date = DateTime.Now
                },

                Action = "Add"
            };

            // Use shared AddEdit view
            return View("AddEdit", vm);
        }

        /* =====================================================================
           EDIT PAYMENT (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays form for editing an existing payment.
        /// </summary>
        /// <param name="id">Primary key of payment</param>
        /// <returns>View with populated ViewModel</returns>
        /// <remarks>
        /// Find() performs efficient primary key lookup.
        /// </remarks>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve existing payment
            var vm = new PaymentViewModel
            {
                Payment = Context.Payments
                    .Include(p => p.Service)
                        .ThenInclude(s => s.Customer)
                    .FirstOrDefault(p => p.Payment_ID == id),

                Action = "Edit"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           SAVE PAYMENT (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Processes form submission for adding or editing a payment.
        /// </summary>
        /// <param name="payment">Bound Payment object</param>
        /// <returns>Redirect or redisplay form</returns>
        /// <remarks>
        /// Determines operation type using Payment_ID.
        /// </remarks>
        [HttpPost]
        public IActionResult Save(Payment payment)
        {
            // Flags used to determine operation type
            bool isPaymentAdd = true;
            bool isPaymentEdit = true;

            // Determine operation based on primary key value
            // ID of 0 means entity has not been saved yet
            if (payment.Payment_ID == 0)
            {
                isPaymentEdit = false;
            }
            else
            {
                isPaymentAdd = false;
            }

            // Validate model before interacting with database
            if (ModelState.IsValid)
            {
                // Add operation
                if (isPaymentAdd)
                {
                    // Marks entity for insertion
                    Context.Payments.Add(payment);
                }

                // Edit operation
                if (isPaymentEdit)
                {
                    // Marks entity for update
                    Context.Payments.Update(payment);
                }

                // Commit all changes in one transaction
                Context.SaveChanges();

                // Redirect to prevent duplicate form submission
                return RedirectToAction("List");
            }
            else
            {
                // Validation failed, must rebuild ViewModel
                // because HTTP requests do not preserve state
                var vm = new PaymentViewModel
                {
                    Payment = payment,

                    Action = isPaymentAdd ? "Add" : "Edit"
                };

                return View("AddEdit", vm);
            }
        }

        ///* =====================================================================
        //   DELETE PAYMENT (HTTP GET)
        //   ===================================================================== */

        ///// <summary>
        ///// Displays confirmation page for deleting a payment.
        ///// </summary>
        ///// <param name="id">Payment ID</param>
        ///// <returns>View with payment and related data</returns>
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    // Retrieve payment with related customer information
        //    // FirstOrDefault avoids exception if not found
        //    var payment = Context.Payments
        //                .Include(p => p.Service)
        //                .FirstOrDefault(p => p.Payment_ID == id);

        //    return View(payment);
        //}

        ///* =====================================================================
        //   DELETE PAYMENT (HTTP POST)
        //   ===================================================================== */

        ///// <summary>
        ///// Deletes a payment after confirmation.
        ///// </summary>
        ///// <param name="payment">Payment entity</param>
        ///// <returns>Redirect to List</returns>
        ///// <remarks>
        ///// Prevents deletion if record is referenced elsewhere.
        ///// </remarks>
        //[HttpPost]
        //public IActionResult Delete(Payment payment)
        //{
        //    // Check if payment is referenced in related table
        //    //var isUsed = Context.Services
        //    //    .Any(ps => ps.Payment_ID == payment.Payment_ID);

        //    //if (isUsed)
        //    //{
        //    //    // TempData persists across redirect
        //    //    TempData["Error"] =
        //    //        "Cannot delete payment assigned to a service event. Delete those service events first.";

        //    //    return RedirectToAction("List");
        //    //}

        //    // Mark entity for deletion
        //    Context.Payments.Remove(payment);

        //    // Execute delete in database
        //    Context.SaveChanges();

        //    return RedirectToAction("List");
        //}
    }
}
