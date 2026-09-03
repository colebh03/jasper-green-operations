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

        public ServiceController(JasperGreenDbContext ctx, PdfMyHtmlService pdfService, RazorViewToStringRenderer viewRenderer)
        {
            Context = ctx;

            PdfService = pdfService;

            ViewRenderer = viewRenderer;
        }

        public IActionResult Index() => RedirectToAction("List");       
                   
        [Route("services/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, string sortColumn = "date", string sortDirection = "desc")
        {
            // Load related data for filtering, sorting, display, and payment status
            IQueryable<Service> query = Context.Services
                .Include(p => p.Customer)
                .Include(p => p.Crew)
                    .ThenInclude(c => c.Foreman)
                .Include(p => p.Property)
                .Include(p => p.Payment);
            
            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Cust_ID == id);
            }
            
            if (filter == "property" && id != null)
            {
                query = query.Where(p => p.Property_ID == id);
            }
            
            if (filter == "crew" && id != null)
            {
                query = query.Where(p => p.Crew_ID == id);
            }

            // Date and fee defaulted to descending order, other columns default to ascending
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
                
                _ => isAsc
                    ? query.OrderBy(p => p.Service_Date)
                    : query.OrderByDescending(p => p.Service_Date)
            };
            
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
        
        [HttpGet]
        [Route("services/getcustomer")]
        public IActionResult GetCustomer()
        {           
            var vm = new CustomerFilterViewModel
            {
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList()
            };

            return View(vm);
        }
        
        [HttpPost]
        [Route("services/getcustomer")]
        public IActionResult GetCustomer(CustomerFilterViewModel vm)
        {            
            if (!ModelState.IsValid)
            {
                // Repopulate the dropdown after validation fails
                vm.Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList();

                return View(vm);
            }
            
            return RedirectToAction("List", new { filter = "customer", id = vm.Cust_ID });
        }
        
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
        
        [HttpGet]
        public IActionResult Add()
        {
            var vm = new ServiceViewModel
            {                
                Service = new Service { Service_Date = DateTime.Now },
                
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
        
        [HttpGet]
        public IActionResult Edit(int id)
        {            
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
                
        [HttpPost]
        public IActionResult Save(Service Service)
        {            
            bool isServiceAdd = true;
            bool isServiceEdit = true;
            
            if (Service.Service_ID == 0)
            {
                isServiceEdit = false;
            }
            else
            {
                isServiceAdd = false;
            }
            
            var property = Context.Properties
                .FirstOrDefault(p => p.Property_ID == Service.Property_ID);

            
            // Service fee cannot be billed below property's standard service rate
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
                
                Context.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {                
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
        
        [HttpGet]
        public IActionResult Delete(int id)
        {            
            var Service = Context.Services
                .Include(ps => ps.Property)
                .Include(ps => ps.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(ps => ps.Service_ID == id);

            return View(Service);
        }
       
        [HttpPost]
        public IActionResult Delete(Service Service)
        {            
            Context.Services.Remove(Service);
            
            Context.SaveChanges();

            return RedirectToAction("List");
        }
        
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
        
        [HttpGet]
        [Route("services/invoice/{id}")]
        public IActionResult Invoice(int id)
        {            
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Property)
                .Include(s => s.Payment)
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(s => s.Service_ID == id);
            
            if (service == null)
            {
                return RedirectToAction("List");
            }
            
            ViewBag.InvoiceNumber =
                $"INV-{service.Service_ID:D6}";

            return View(service);
        }
        
        [HttpGet]
        [Route("services/invoicepdf/{id}")]
        public async Task<IActionResult> InvoicePdf(int id)
        {
            var totalSw = Stopwatch.StartNew();
            
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Property)
                .Include(s => s.Payment)
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .FirstOrDefault(s => s.Service_ID == id);
            
            if (service == null)
            {
                return RedirectToAction("List");
            }
            
            ViewBag.InvoiceNumber =
                $"INV-{service.Service_ID:D6}";
            
            var renderSw = Stopwatch.StartNew();

            // Render Razor invoice view into HTML string before sending to the PDF service
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
            
            var pdfSw = Stopwatch.StartNew();

            // Convert the rendered invoice through the PDF servuce
            byte[] pdfBytes =
                await PdfService.GeneratePdfAsync(html);

            pdfSw.Stop();

            Debug.WriteLine(
                $"PDF generation time: {pdfSw.ElapsedMilliseconds}ms");

            totalSw.Stop();

            Debug.WriteLine(
                $"TOTAL InvoicePdf time: {totalSw.ElapsedMilliseconds}ms");
            
            return File(
                pdfBytes,
                "application/pdf",
                $"Invoice-{service.Service_ID}.pdf");
        }
    }
}
