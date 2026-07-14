using App.Models;
using App.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace App.Extentions;

public static class MyExtentions
{
    extension(HttpContext c) {
        public User? GetUser() => c.Items["user"] as User;
    }

    public const string HX_REQUEST_HEADER = "HX-Request";
    extension(HttpRequest req) {
        public bool IsHtmx() => req.Headers[HX_REQUEST_HEADER] == "true";
    }

    public const string MAIN_LAYOUT_NAME = "main";
    public static readonly ViewPath layout_path = new ViewPath{ path_prefix = "/Views/Layouts/" };

    extension(ViewDataDictionary view_data) {
        public void SetLayout(string layout_name = MAIN_LAYOUT_NAME) => view_data["layout"] = layout_path.GetPath(layout_name);

        public string GetCurrentPath() => (string?)view_data["current_path"] ?? "";
        public void SetCurrentPath(string path) => view_data["current_path"] = path;

        public string GetSavedLocation() => Uri.UnescapeDataString((string?)view_data["saved_location"] ?? "");
        public void SaveLocation(string url) => view_data["saved_location"] = Uri.EscapeDataString(url);
    }
}
