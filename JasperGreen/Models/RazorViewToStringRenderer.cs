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
            controller.ViewData.Model = model;

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

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return writer.ToString();
        }
    }
}