using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JasperGreen.Models
{
    public class RazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;

        private readonly ITempDataProvider _tempDataProvider;

        private readonly IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;

            _tempDataProvider = tempDataProvider;

            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync(
            Controller controller,
            string viewName,
            object model)
        {
            // Supply the model to the Razor view before rendering
            controller.ViewData.Model = model;

            // Capture the rendered HTML in memory instead of writing it to the HTTP response
            using var writer = new StringWriter();

            var viewResult = _viewEngine.FindView(
                controller.ControllerContext,
                viewName,
                false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException(
                    $"View '{viewName}' was not found.");
            }

            // Build the Razor rendering context using the controller's existing view and TempData state
            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions());

            // Render the Razor view into the StringWriter and return the generated HTML
            await viewResult.View.RenderAsync(viewContext);

            return writer.ToString();
        }
    }
}