/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      HomeController.cs

PURPOSE:      Handles requests for the public Jasper Green pages, including
              Home, About, and Contact views.

INPUT:        HTTP requests routed to controller actions (Index, About, Contact).

PROCESS:      Maps incoming requests to the appropriate public-facing view.

OUTPUT:       ViewResults that render the public Home, About, and Contact pages.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;

namespace JasperGreen.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}
