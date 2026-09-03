using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using Microsoft.AspNetCore.Authorization;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private JasperGreenDbContext Context { get; set; }
        
        public PaymentController(JasperGreenDbContext ctx) => Context = ctx;
        
        public IActionResult Index() => RedirectToAction("List");

        [Route("payments/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, DateTime? startDate = null, DateTime? endDate = null, string sortColumn = "date", string sortDirection = "desc")
        {
            IQueryable<Payment> query = Context.Payments
                .Include(p => p.Service)
                    .ThenInclude(s => s.Customer)
                .Include(p => p.Service)
                    .ThenInclude(s => s.Property);          
            
            if (filter == "date" &&
                startDate != null &&
                endDate != null)
            {
                query = query.Where(p =>
                    p.Payment_Date >= startDate &&
                    p.Payment_Date < endDate.Value.AddDays(1));
            }

            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Service.Cust_ID == id);
            }
            
            // Date, Amount, and ID naturally default to DESC
            // Customer and Method naturally default to ASC            
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
        
        [HttpGet]
        [Route("payments/getdate")]
        public IActionResult GetDate()
        {
            var vm = new DateFilterViewModel();

            return View(vm);
        }
        
        [HttpPost]
        [Route("payments/getdate")]
        public IActionResult GetDate(DateFilterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            return RedirectToAction("List",
                new
                {
                    filter = "date",
                    startDate = vm.StartDate,
                    endDate = vm.EndDate
                });
        }
        
        [HttpGet]
        [Route("payments/getcustomer")]
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
        [Route("payments/getcustomer")]
        public IActionResult GetCustomer(CustomerFilterViewModel vm)
        {            
            if (!ModelState.IsValid)
            {                
                vm.Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList();

                return View(vm);
            }
            
            return RedirectToAction("List",
                new
                {
                    filter = "customer",
                    id = vm.Cust_ID
                });
        }
        
        [HttpGet]
        public IActionResult Add(int id)
        {
            // Retrieve associated service
            var service = Context.Services
                .Include(s => s.Customer)
                .Include(s => s.Payment)
                .FirstOrDefault(s => s.Service_ID == id);
            
            if (service == null)
            {
                return RedirectToAction("List", "Service");
            }

            if (service.Payment != null)
            {
                return RedirectToAction("List", "Service");
            }
            
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
            
            return View("AddEdit", vm);
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {            
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
        
        [HttpPost]
        public IActionResult Save(Payment payment)
        {            
            bool isPaymentAdd = true;
            bool isPaymentEdit = true;
            
            if (payment.Payment_ID == 0)
            {
                isPaymentEdit = false;
            }
            else
            {
                isPaymentAdd = false;
            }
            
            if (ModelState.IsValid)
            {
                
                if (isPaymentAdd)
                {
                    // Marks entity for insertion
                    Context.Payments.Add(payment);
                }
                
                if (isPaymentEdit)
                {
                    // Marks entity for update
                    Context.Payments.Update(payment);
                }

                Context.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {               
                var vm = new PaymentViewModel
                {
                    Payment = payment,

                    Action = isPaymentAdd ? "Add" : "Edit"
                };

                return View("AddEdit", vm);
            }
        }
    }
}
