namespace App.Helpers;

public class ViewPath {
    public string path_prefix = "";
    public string file_extention = ".cshtml";

    public string GetPath(string name) {
        return Path.GetFullPath($"{path_prefix}/{name}{file_extention}");
    }
}

