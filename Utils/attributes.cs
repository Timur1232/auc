using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace App.Attributes;

// Supporting only russian for now
public enum Language {
    Ru,
}

// TODO: Introduce language changing
public class Title(string title, Language language = Language.Ru) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _ = language;
        if (context.Controller is Controller c) {
            c.ViewData["title"] = $"{Config.APP_NAME} - {title}";
        }
    }
}

public class Htmx(bool requred = true) : ActionMethodSelectorAttribute
{
    public override bool IsValidForRequest(RouteContext route_context, ActionDescriptor action)
    {
        return requred == G.IsHtmx(route_context.HttpContext.Request);
    }
}

public class HtmxRedirect : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Controller is Controller c) {
            if (G.IsHtmx(c.Request)) {
                if (context.Result is RedirectResult r) {
                    c.Response.Headers.Append(G.HX_REDIRECT_HEADER, r.Url);
                    context.Result = new OkResult();
                }
            }
        }
    }
}

public class HtmxLayout : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller c) {
            if (!G.IsHtmx(c.Request)) G.SetLayout(c.ViewData);
        }
    }
}

public class HtmxViewData : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller c) {
            c.ViewBag.htmx = G.IsHtmx(c.Request);
        }
    }
}

public class Json(bool requerd = true) : ActionMethodSelectorAttribute
{
    public override bool IsValidForRequest(RouteContext route_context, ActionDescriptor action)
    {
        return requerd == (route_context.HttpContext.Request.Query["json"] == "true");
    }
}
