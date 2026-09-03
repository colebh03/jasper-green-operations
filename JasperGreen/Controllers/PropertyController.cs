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

        [Route("properties/{filter?}")]
        public IActionResult List(string filter = "all", int? id = null, string sortColumn = "property", string sortDirection = "asc")
        {            
            IQueryable<Property> query = Context.Properties
                .Include(p => p.Customer);

            if (filter == "customer" && id != null)
            {
                query = query.Where(p => p.Cust_ID == id);
            }

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

            string currentFilterText = "";

            if (filter == "customer" && id != null)
            {
                var customer = Context.Customers.Find(id);

                currentFilterText =
                    $"Customer Filter: {customer?.Cust_Name}";
            }

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

        [HttpGet]
        [Route("properties/getcustomer")]
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
        [Route("properties/getcustomer")]
        public IActionResult GetCustomer(CustomerFilterViewModel vm)
        {            
            if (!ModelState.IsValid)
            {
                vm.Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList();

                return View(vm);
            }

            return RedirectToAction("List", new { filter = "customer", id = vm.Cust_ID });
        }

        [HttpGet]
        public IActionResult Add()
        {
            var vm = new PropertyViewModel
            {
                Property = new Property(),
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),
                States = StateHelper.GetStates(), 
                Action = "Add"
            };

            return View("AddEdit", vm);
        }
       
        [HttpGet]
        public IActionResult Edit(int id)
        {           
            var vm = new PropertyViewModel
            {
                Property = Context.Properties.Find(id),
                Customers = Context.Customers
                    .OrderBy(c => c.Cust_Name)
                    .ToList(),
                States = StateHelper.GetStates(),
                Action = "Edit"
            };

            return View("AddEdit", vm);
        }

        [HttpPost]
        public IActionResult Save(Property property)
        {           
            bool isPropertyAdd = true;
            bool isPropertyEdit = true;

            if (property.Property_ID == 0)
            {               
                isPropertyEdit = false;  
            }

            else
            {                
                isPropertyAdd = false;   
            }

            if (ModelState.IsValid)
            {                
                if (isPropertyAdd)
                {                   
                    Context.Properties.Add(property);
                }

                if (isPropertyEdit)
                {                    
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
                    States = StateHelper.GetStates(), 
                    Action = isPropertyAdd ? "Add" : "Edit"
                };

                return View("AddEdit", vm);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {            
            var property = Context.Properties
                        .Include(p => p.Customer)
                        .FirstOrDefault(p => p.Property_ID == id);
            
            return View(property);
        }

        [HttpPost]
        public IActionResult Delete(Property property)
        {           
            var hasServices = Context.Services
                .Any(ps => ps.Property_ID == property.Property_ID);

            // Referential integrity
            if (hasServices)
            {
                TempData["Error"] =
                    "Cannot delete property assigned to a service event. Delete those service events first.";

                return RedirectToAction("List");
            }

            Context.Properties.Remove(property);

            Context.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
