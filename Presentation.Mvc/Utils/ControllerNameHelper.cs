namespace Presentation.Mvc.Utils;

public static class ControllerNameHelper
{
    public static string GetControllerName(this Type controllerType)
    {
        const string controllerSuffix = "Controller";
        var typeName = controllerType.Name;

        if (typeName.EndsWith(controllerSuffix, StringComparison.OrdinalIgnoreCase))
            return typeName.Substring(0, typeName.Length - controllerSuffix.Length);

        return typeName; // Return the full name if it doesn't end with "Controller"
    }
}