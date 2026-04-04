using System.Reflection;

namespace EyeTracking.Utilities;

internal static class EmbeddedResourceUtilities
{
    internal static byte[] LoadBytesFromAssembly(Assembly assembly, string name)
    {
        string[] manifestResources = assembly.GetManifestResourceNames();

        if (!manifestResources.Contains(name))
        {
            return null;
        }

        using Stream str = assembly.GetManifestResourceStream(name);
        using MemoryStream memoryStream = new();

        if (str != null) 
            str.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}