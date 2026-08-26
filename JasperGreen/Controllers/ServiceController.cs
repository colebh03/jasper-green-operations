/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PropertyController.cs

PURPOSE:      
              This controller manages all operations related to service events
              (Service entities). It handles listing, filtering, creation,
              editing, validation, and deletion of service records while coordinating
              related data such as Customers, Crews, Properties, and Payments.

INPUT:        
              - HTTP GET and POST requests from user interactions
              - Route parameters (filter type, IDs)
              - Form-bound Service objects
              - ViewModel selections (Customer, Property, Crew filters)

PROCESS:      
              - Builds EF Core queries with optional filtering
              - Uses eager loading to include related entities
              - Validates business rules (such as service fee minimums)
              - Determines Add vs Edit operations based on primary key
              - Persists data changes using Entity Framework Core
              - Reconstructs ViewModels when validation fails

OUTPUT:       
              - Returns Razor Views (List, AddEdit, filter selection pages)
              - Redirects to filtered or full list views
              - Provides validation feedback via ModelState

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private JasperGreenDbContext Context { get; set; }

        private PdfMyHtmlService PdfService { get; set; }

        private RazorViewToStringRenderer ViewRenderer { get; set; }

        public ServiceController(
            JasperGreenDbContext ctx,
            PdfMyHtmlService pdfService,
            RazorViewToStringRenderer viewRenderer)
        {
            Context = ctx;

            PdfService = pdfService;

            ViewRenderer = viewRenderer;
        }

        public IActionResult Index() => RedirectToAction("List");

        /* =====================================================================
          LIST SERVICE
          ===================================================================== */

        /// <summary>
        /// Retrieves and displays service records with optional filtering.
        /// </summary>
        /// <param name="filter">
        /// Specifies filter type: "all", "customer", "property", or "crew".
        /// </param>
        /// <param name="id">
        /// Identifier used for filtering (Customer_ID, Property_ID, or Crew_ID).
        /// </param>
        /// <returns>View containing filtered or full list of services</returns>
        /// <remarks>
        /// Builds a base IQueryable and conditionally applies filters before execution.
        /// </remarks>
        [Route("services/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, string sortColumn = "date", string sortDirection = "desc")
        {
            // -----------------------------------------------------------------
            // Build base query with eager loading:
            // - Customer: who requested service
            // - Crew + Foreman: who performed service
            // - Property: where service occurred
            // - Payment: associated payment record
            // IQueryable allows deferred execution until ToList() is called
            // -----------------------------------------------------------------
            IQueryable<Service> query = Context.Services
                .Include(p => p.Customer)
                .Include(p => p.Crew)
                    .ThenInclude(c => c.Foreman)
                .Include(p => p.Property)
                .Include(p => p.Payment);
            //.OrderBy(p => p.Service_Date); Deleted so it doesn't overide new dynamic sorting

            // Apply filters based on which button was used

            // Filter by Customer
            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Cust_ID == id);
            }

            // Filter by Property
            if (filter == "property" && id != null)
            {
                query = query.Where(p => p.Property_ID == id);
            }

            // Filter by Crew
            if (filter == "crew" && id != null)
            {
                query = query.Where(p => p.Crew_ID == id);
            }

            //Dynamic Filtering Code
            bool isAsc = sortColumn switch
            {
                "customer" => sortDirection == "asc",
                "property" => sortDirection == "asc",
                "crew" => sortDirection == "asc",

                "date" => sortDirection != "desc",
                "fee" => sortDirection != "desc",

                _ => sortDirection == "asc"
            };

            query = sortColumn switch
            {
                "date" => isAsc
                    ? query.OrderBy(p => p.Service_Date)
                    : query.OrderByDescending(p => p.Service_Date),

                "customer" => isAsc
                    ? query.OrderBy(p => p.Customer.Cust_Name)
                    : query.OrderByDescending(p => p.Customer.Cust_Name),

                "property" => isAsc
                    ? query.OrderBy(p => p.Property.Property_Address + " " + p.Property.Property_City)
                    : query.OrderByDescending(p => p.Property.Property_Address + " " + p.Property.Property_City),

                "crew" => isAsc
                    ? query.OrderBy(p => p.Crew.Foreman)
                    : query.OrderByDescending(p => p.Crew.Foreman),

                "fee" => isAsc
                    ? query.OrderBy(p => p.Service_Fee)
                    : query.OrderByDescending(p => p.Service_Fee),

                // Default Fallback
                _ => isAsc
                    ? query.OrderBy(p => p.Service_Date)
                    : query.OrderByDescending(p => p.Service_Date)
            };

            // Current Filtering String Creator
            string currentFilterText = "";

            if (filter == "customer" && id != null)
            {
                var customer = Context.Customers.Find(id);

                currentFilterText =
                    $"Customer Filter: {customer?.Cust_Name}";
            }
            else if (filter == "property" && id != null)
            {
                var property = Context.Properties.Find(id);

                currentFilterText =
                    $"Property Filter: {property?.Property_Full_Address}";
            }
            else if (filter == "crew" && id != null)
            {
                var crew = Context.Crews
                    .Include(c => c.Foreman)
                    .FirstOrDefault(c => c.Crew_ID == id);

                currentFilterText =
                    $"Crew Filter: {crew?.Foreman}";
            }


            //Create ViewModel
            var vm = new ServiceListViewModel
            {
                Services = query.ToList(),
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
        /// Displays form to select a customer for filtering services.
        /// </summary>
        /// <returns>View with list of customers</returns>
        [HttpGet]
        [Route("services/getcustomer")]
        public IActionResult GetCustomer()
        {
           //Populate dropdown list of customers
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
        [Route("services/getcustomer")]
        public IActionResult GetCustomer(CustomerFilterViewModel vm)
        {
            //Validation ensures a selection was made
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
            FILTER BY PROPERTY
            ===================================================================== */

        /// <summary>
        /// Displays form to select a property for filtering services.
        /// </summary>
        /// <returns>View with list of properties</returns>
        [HttpGet]
        [Route("services/getproperty")]
        public IActionResult GetProperty()
        {
            var vm = new PropertyFilterViewModel
            {
                Properties = Context.Properties
                    .OrderBy(p => p.Property_Address)
                    .ToList()
            };

            return View(vm);
        }

        /// <summary>
        /// Processes selected property filter and redirects to filtered list.
        /// </summary>
        /// <param name="vm">ViewModel containing selected Property ID</param>
        /// <returns>Redirect to filtered List view</returns>
        [HttpPost]
        [Route("services/getproperty")]
        public IActionResult GetProperty(PropertyFilterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Properties = Context.Properties
                    .OrderBy(c => c.Property_Address)
                    .ToList();

                return View(vm);
            }

            return RedirectToAction("List", new { filter = "property", id = vm.Property_ID });
        }

        /* =====================================================================
          FILTER BY CREW
          ===================================================================== */

        /// <summary>
        /// Displays form to select a crew for filtering services.
        /// </summary>
        /// <returns>View with list of crews and related members</returns>
        [HttpGet]
        [Route("services/getcrew")]
        public IActionResult GetCrew()
        {
            var vm = new CrewFilterViewModel
            {
                Crews = Context.Crews
                    .Include(c => c.Foreman)
                    .Include(c => c.CrewMember1)
                    .Include(c => c.CrewMember2)
                    .OrderBy(c => c.Foreman.Emp_Last_Name)
                    .ToList()
            };
            return View(vm);
        }

        /// <summary>
        /// Processes selected crew filter and redirects to filtered list.
        /// </summary>
        /// <param name="vm">ViewModel containing selected Crew ID</param>
        /// <returns>Redirect to filtered List view</returns>
        [HttpPost]
        [Route("services/getcrew")]
        public IActionResult GetCrew(CrewFilterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Crews = Context.Crews
                    .Include(c => c.Foreman)
                    .Include(c => c.CrewMember1)
                    .Include(c => c.CrewMember2)
                    .OrderBy(c => c.Foreman.Emp_Last_Name)
                    .ToList();

                return View(vm);
            }

            return RedirectToAction("List", new { filter = "crew", id = vm.Crew_ID });
        }

        /* =====================================================================
           ADD SERVICE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays form to create a new service record.
        /// </summary>
        /// <returns>View with initialized ServiceViewModel</returns>
        [HttpGet]
        public IActionResult Add()
        {
            var vm = new ServiceViewModel
            {
                // Default Service_Date set to current system time
                Service = new Service { Service_Date = DateTime.Now },

                // Populate dropdown lists for selection
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),

                Crews = Context.Crews
                    .Include(c => c.Foreman)
                    .Include(c => c.CrewMember1)
                    .Include(c => c.CrewMember2)
                    .OrderBy(c => c.Foreman.Emp_Last_Name)
                    .ToList(),

                Properties = Context.Properties
                    .OrderBy(p => p.Property_Address)
                    .ThenBy(p => p.Property_City)
                    .ToList(),

                Action = "Add"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
           EDIT SERVICE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays form to edit an existing service record.
        /// </summary>
        /// <param name="id">Service_ID of the record</param>
        /// <returns>View with populated ViewModel</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve service with payment relationship
            var service = Context.Services
                .Include(s => s.Payment)
                .FirstOrDefault(s => s.Service_ID == id);

            // Prevent editing finalized/paid services
            if (service == null || service.Payment != null)
            {
                return RedirectToAction("List");
            }

            var vm = new ServiceViewModel
            {
                Service = service,

                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),

                Crews = Context.Crews
                    .Include(c => c.Foreman)
                    .Include(c => c.CrewMember1)
                    .Include(c => c.CrewMember2)
                    .OrderBy(c => c.Foreman.Emp_Last_Name)
                    .ToList(),

                Properties = Context.Properties
                    .OrderBy(p => p.Property_Address)
                    .ThenBy(p => p.Property_City)
                    .ToList(),

                Action = "Edit"
            };

            return View("AddEdit", vm);
        }

        /* =====================================================================
          SAVE SERVICE (HTTP POST)
          ===================================================================== */

        /// <summary>
        /// Processes form submission to add or update a service record.
        /// </summary>
        /// <param name="Service">Bound Service entity</param>
        /// <returns>Redirect or redisplay form</returns>
        [HttpPost]
        public IActionResult Save(Service Service)
        {
            // Flags determine INSERT vs UPDATE operation
            bool isServiceAdd = true;
            bool isServiceEdit = true;

            // Determine operation type using primary key
            if (Service.Service_ID == 0)
            {
                isServiceEdit = false;
            }
            else
            {
                isServiceAdd = false;
            }

            // Retrieve associated property to enforce business rule
            var property = Context.Properties
                .FirstOrDefault(p => p.Property_ID == Service.Property_ID);

            // Business rule validation:
            // Service fee cannot be below property's standard rate
            if (property != null && Service.Service_Fee < property.Property_Service_Fee)
            {
                ModelState.AddModelError("Service.Service_Fee",
                    $"Service fee must be at least {property.Property_Service_Fee:C} (property rate).");
            }

            if (ModelState.IsValid)
            {
                if (isServiceAdd)
                {
                    Context.Services.Add(Service);
                }

                if (isServiceEdit)
                {
                    Context.Services.Update(Service);
                }

                // Persist changes to database
                Context.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                // Rebuild ViewModel after validation failure
                var vm = new ServiceViewModel
                {
                    Service = Service,

                    Customers = Context.Customers
                        .OrderBy(c => c.Cust_Name)
                        .ToList(),

                    Crews = Context.Crews
                        .Include(c => c.Foreman)
                        .Include(c => c.CrewMember1)
                        .Include(c => c.CrewMember2)
                        .OrderBy(c => c.Foreman.Emp_Last_Name)
                        .ToList(),

                    Properties = Context.Properties
                        .OrderBy(p => p.Property_Address)
                        .ThenBy(p => p.Property_City)
                        .ToList(),

                    Action = isServiceAdd ? "Add" : "Edit"
                };

                return View("AddEdit", vm);
            }
        }

        /* =====================================================================
           DELETE SERVICE (HTTP GET)
           ===================================================================== */

        /// <summary>
        /// Displays confirmation page for deleting a service record.
        /// </summary>
        /// <param name="id">Service_ID</param>
        /// <returns>View with service details</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve service with related entities for display
            var Service = Context.Services
                .Include(ps => ps.Property)
                .Include(ps => ps.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(ps => ps.Service_ID == id);

            return View(Service);
        }

        /* =====================================================================
           DELETE SERVICE (HTTP POST)
           ===================================================================== */

        /// <summary>
        /// Deletes a service record after confirmation.
        /// </summary>
        /// <param name="Service">Entity to delete</param>
        /// <returns>Redirect to List</returns>
        [HttpPost]
        public IActionResult Delete(Service Service)
        {
            // Mark entity as Deleted
            Context.Services.Remove(Service);

            // Execute DELETE operation
            Context.SaveChanges();

            return RedirectToAction("List");
        }

        /* =====================================================================
           CONTROLLER ENDPOINT (HTTP GET)
           ===================================================================== */
        [HttpGet]
        public JsonResult GetPropertiesByCustomer(int customerId)
        {
            var properties = Context.Properties
                .Where(p => p.Cust_ID == customerId)
                .OrderBy(p => p.Property_Address)
                .Select(p => new
                {
                    property_ID = p.Property_ID,
                    fullAddress = p.Property_Full_Address
                })
                .ToList();

            return Json(properties);
        }

        /* =====================================================================
           VIEW INVOICE
           ===================================================================== */

        /// <summary>
        /// Displays printable invoice page for a completed service.
        /// </summary>
        /// <param name = "id" > Service_ID </ param >
        /// < returns > Invoice view populated with related data</returns>
        [HttpGet]
        [Route("services/invoice/{id}")]
        public IActionResult Invoice(int id)
        {
            // Retrieve full invoice data
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Property)
                .Include(s => s.Payment)
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(s => s.Service_ID == id);

            // Prevent invalid invoice access
            if (service == null)
            {
                return RedirectToAction("List");
            }

            // Generate business-facing invoice number
            ViewBag.InvoiceNumber =
                $"INV-{service.Service_ID:D6}";

            return View(service);
        }



        /* =====================================================================
   DOWNLOAD INVOICE PDF
   ===================================================================== */

        /// <summary>
        /// Generates PDF invoice using pdfmyhtml API.
        /// </summary>
        /// <param name="id">Service_ID</param>
        /// <returns>Generated PDF file</returns>
        [HttpGet]
        [Route("services/invoicepdf/{id}")]
        public async Task<IActionResult> InvoicePdf(int id)
        {
            var totalSw = Stopwatch.StartNew();

            // Retrieve full invoice data
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Property)
                .Include(s => s.Payment)
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(s => s.Service_ID == id);

            // Prevent invalid invoice access
            if (service == null)
            {
                return RedirectToAction("List");
            }

            // Generate invoice number
            ViewBag.InvoiceNumber =
                $"INV-{service.Service_ID:D6}";

            // Render Invoice.cshtml into HTML string
            var renderSw = Stopwatch.StartNew();

            string html =
                await ViewRenderer.RenderViewToStringAsync(
                    this,
                    "Invoice",
                    service);

            renderSw.Stop();

            Debug.WriteLine(
                $"Invoice render time: {renderSw.ElapsedMilliseconds}ms");

            Debug.WriteLine(
                $"Invoice HTML size: {html.Length:N0} characters");

            // Generate PDF using service
            var pdfSw = Stopwatch.StartNew();

            byte[] pdfBytes =
                await PdfService.GeneratePdfAsync(html);

            pdfSw.Stop();

            Debug.WriteLine(
                $"PDF generation time: {pdfSw.ElapsedMilliseconds}ms");

            totalSw.Stop();

            Debug.WriteLine(
                $"TOTAL InvoicePdf time: {totalSw.ElapsedMilliseconds}ms");

            // Return downloadable PDF
            return File(
                pdfBytes,
                "application/pdf",
                $"Invoice-{service.Service_ID}.pdf");
        }


    }
}
