using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class ControllerExtensions
{
	public static async Task<string> RenderViewAsync<TModel>(this Controller controller, [AspMvcMaster] string viewName, TModel model, bool partial = false)
	{
		if (string.IsNullOrEmpty(viewName))
		{
			viewName = controller.ControllerContext.ActionDescriptor.ActionName;
		}

		controller.ViewData.Model = model;

		using (var writer = new StringWriter())
		{
			IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
			ViewEngineResult viewResult = viewEngine.GetView(viewName, viewName, !partial);

			if (viewResult.Success == false)
			{
				return $"A view with the name {viewName} could not be found";
			}

			ViewContext viewContext = new ViewContext(
				controller.ControllerContext,
				viewResult.View,
				controller.ViewData,
				controller.TempData,
				writer,
				new HtmlHelperOptions()
			);

			await viewResult.View.RenderAsync(viewContext);

			return writer.GetStringBuilder().ToString();
		}
	}

    public static ViewResult View<T>(this PageController<T> controller, ContentData currentContent, object model)
        where T : PageData
    {
        var view = BuildViewPath(currentContent, true);

        if (model != null)
        {
            controller.ViewData.Model = model;
        }

        var viewResult = new ViewResult
        {
            ViewName = view,
            ViewData = controller.ViewData,
            TempData = controller.TempData
        };

        return viewResult;
    }

    public static ViewResult View<T>(this BlockComponent<T> controller, ContentData currentContent, object model)
		where T : BlockData
	{
		var view = BuildViewPath(currentContent);

		if (model != null)
		{
			controller.ViewData.Model = model;
		}

		var viewResult = new ViewResult
		{
			ViewName = view,
			ViewData = controller.ViewData,
			TempData = controller.TempData
		};

		return viewResult;
	}

	public static ViewViewComponentResult View<T>(this AsyncBlockComponent<T> controller, ContentData currentContent,
		object model) where T : BlockData
	{
		var view = BuildViewPath(currentContent);

		if (model != null)
		{
			controller.ViewData.Model = model;
		}

		var viewResult = new ViewViewComponentResult
		{
			ViewName = view,
			ViewData = controller.ViewData,
			TempData = controller.TempData
		};

		return viewResult;
	}

	public static PartialViewResult PartialView<T>(this BlockComponent<T> controller, ContentData currentContent,
		object model) where T : BlockData
	{
		var view = BuildViewPath(currentContent);

		if (model != null)
		{
			controller.ViewData.Model = model;
		}

		var partialViewResult = new PartialViewResult
		{
			ViewName = view,
			ViewData = controller.ViewData,
			TempData = controller.TempData
		};

		return partialViewResult;
	}

	private static string BuildViewPath(ContentData currentContent, bool isPage = false)
	{
		var nameSpace = currentContent.GetOriginalType().Namespace;
		var className = currentContent.GetOriginalType().Name;

		var path = nameSpace.Replace("ChildFund.Features", "~/Features").Replace(".", "/");
		return isPage ? $"{path}/Index.cshtml" : $"{path}/{className}.cshtml";
	}
}