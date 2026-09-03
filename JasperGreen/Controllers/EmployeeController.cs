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
        
        [Route("employees")]
        public IActionResult List(string sortColumn = "name", string sortDirection = "asc")
        {
            IQueryable<Employee> query = Context.Employees;            
            
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
            
            var vm = new EmployeeListViewModel
            {
                Employees = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add"; 

            return View("AddEdit", new Employee());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";  

            var employee = Context.Employees.Find(id);           

            return View("AddEdit", employee);
        }
        
        [HttpPost]
        public IActionResult Save(Employee employee)
        {            
            bool isEmployeeAdd = true;
            bool isEmployeeEdit = true;
            
            if (employee.Emp_ID == 0)
            {                
                ViewBag.Action = "Add";
                isEmployeeEdit = false;  
            }

            else
            {                
                ViewBag.Action = "Edit";
                isEmployeeAdd = false;  
            }

            if (ModelState.IsValid)
            {                
                if (isEmployeeAdd)
                {                    
                    Context.Employees.Add(employee);
                }
                if (isEmployeeEdit)
                {                    
                    Context.Employees.Update(employee);
                }
                
                Context.SaveChanges();
               
                return RedirectToAction("List");
            }

            else
            {               
                return View("AddEdit", employee);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {           
            var employee = Context.Employees.Find(id); 

            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            // Referential integrity below
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
            
            Context.Employees.Remove(employee);

            Context.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
