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
        private JasperGreenDbContext Context { get; set; }
        
        public CrewController(JasperGreenDbContext ctx) => Context = ctx;
        
        public IActionResult Index() => RedirectToAction("List");
        
        [Route("crews")]
        public IActionResult List(string sortColumn = "foreman", string sortDirection = "asc")
        {
            // Load related data for display and sorting
            IQueryable<Crew> query = Context.Crews
                .Include(c => c.Foreman)
                .Include(c => c.CrewMember1)
                .Include(c => c.CrewMember2);
            
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
           
            var vm = new CrewListViewModel
            {
                Crews = query.ToList(),
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            return View(vm);
        }
        
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
        
        [HttpPost]
        public IActionResult Save(Crew crew)
        {            
            bool isCrewAdd = true;
            bool isCrewEdit = true;
            
            if (crew.Crew_ID == 0)            {
                
                isCrewEdit = false;  
            }

            else
            {                
                isCrewAdd = false;   
            }

            if (ModelState.IsValid)
            {              
                if (isCrewAdd)
                {                    
                    Context.Crews.Add(crew);
                }
                if (isCrewEdit)
                {                    
                    Context.Crews.Update(crew);
                }
                
                Context.SaveChanges();
                
                return RedirectToAction("List");

            }

            else
            {                
                var vm = new CrewViewModel
                {
                    Crew = crew,
                    Employees = Context.Employees
                        .OrderBy(c => c.Emp_First_Name)
                        .ToList(),
                    Action = isCrewAdd ? "Add" : "Edit"
                };

                return View("AddEdit", vm);
            }
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {            
            var crew = Context.Crews
                 .Include(c => c.Foreman)
                .Include(c => c.CrewMember1)
                .Include(c => c.CrewMember2)
                .FirstOrDefault(c => c.Crew_ID == id);
            
            return View(crew);
        }

        [HttpPost]
        public IActionResult Delete(Crew crew)
        {
            var isInService = Context.Services.Any(ps =>
                ps.Crew_ID == crew.Crew_ID);

            // Referential integrity
            if (isInService)
            {
                TempData["Error"] =
                    "Cannot delete crew assigned to a service event. Delete those service events first.";

                return RedirectToAction("List");
            }
            
            Context.Crews.Remove(crew);
            
            Context.SaveChanges();
            
            return RedirectToAction("List");
        }
    }
}
