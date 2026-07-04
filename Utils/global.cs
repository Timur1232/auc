using Microsoft.AspNetCore.Mvc.ViewFeatures;
using App.Helpers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
namespace App;

static class G
{
    public const string MAIN_LAYOUT_NAME = "main";
    public static readonly ViewPath layout_path = new ViewPath{ path_prefix = "/Views/Layouts/" };

    public const string HX_REQUEST_HEADER = "HX-Request";
    public const string HX_REDIRECT_HEADER = "HX-Redirect";

    public static bool IsHtmx(HttpRequest req)
    {
        return req.Headers[HX_REQUEST_HEADER] == "true";
    }

    public static void SetLayout(ViewDataDictionary view_data, string layout_name = MAIN_LAYOUT_NAME)
    {
        view_data["layout"] = layout_path.GetPath(layout_name);
    }

    [DoesNotReturn]
    public static void Todo(string? message = null)
    {
        throw new NotImplementedException($"TODO: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static T Todo<T>(string? message = null)
    {
        throw new NotImplementedException($"TODO: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static void Unreachable(string? message = null)
    {
        throw new UnreachableException($"UNREACHABLE: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static T Unreachable<T>(string? message = null)
    {
        throw new UnreachableException($"UNREACHABLE: {message ?? "no message"}");
    }
}
