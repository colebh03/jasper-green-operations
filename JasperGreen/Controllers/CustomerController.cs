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
        private JasperGreenDbContext Context { get; set; }
        
        public CustomerController(JasperGreenDbContext ctx) => Context = ctx;
        
        public IActionResult Index() => RedirectToAction("List");
        
        [Route("customers")]
        public IActionResult List(string sortColumn = "name", string sortDirection = "asc")
        {
            IQueryable<Customer> query = Context.Customers;
            
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
            
            var vm = new CustomerListViewModel
            {
                Customers = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            
            ViewBag.States = StateHelper.GetStates();

            return View("AddEdit", new Customer());
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";

            var customer = Context.Customers.Find(id);     

            ViewBag.States = StateHelper.GetStates();

            return View("AddEdit", customer);
        }
        
        [HttpPost]
        public IActionResult Save(Customer customer)
        {            
            bool isCustomerAdd = true;
            bool isCustomerEdit = true;
            
            if (customer.Cust_ID == 0)
            {
                
                ViewBag.Action = "Add";
                isCustomerEdit = false;  
            }

            else
            {                
                ViewBag.Action = "Edit";
                isCustomerAdd = false;   
            }

            if (ModelState.IsValid)
            {               
                if (isCustomerAdd)
                {                    
                    Context.Customers.Add(customer);
                }
                if (isCustomerEdit)
                {                   
                    Context.Customers.Update(customer);
                }
                
                Context.SaveChanges();

                if (isCustomerAdd)
                {
                    TempData["message"] = $"{customer.Cust_Name} was added.";
                }
                else 
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
        
        [HttpGet]
        public IActionResult Delete(int id)
        {            
            var customer = Context.Customers.Find(id); 
            
            return View(customer);
        }
        
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            var isInProperty = Context.Properties.Any(p =>
                p.Cust_ID == customer.Cust_ID);

            var isInService = Context.Services.Any(ps =>
                ps.Cust_ID == customer.Cust_ID);

            // Referential integrity
            if (isInProperty || isInService)
            {
                TempData["Error"] =
                    "Cannot delete customer with existing properties, service events, or payments. Delete those records first.";

                return RedirectToAction("List");
            }
            
            Context.Customers.Remove(customer);
            
            Context.SaveChanges();
            
            return RedirectToAction("List");        
        }
    }
}
