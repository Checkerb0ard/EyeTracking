using System.Reflection;
using UnityEngine;

namespace EyeTracking;

public static class Utils
{
    public static void LoadAllValid<T>(Assembly assembly, Action<Type> runOnValid)
    {
        string asmName = assembly.FullName;
        if (asmName != null && asmName.Contains("System"))
            return;

        foreach (Type type in assembly.GetTypes())
        {
            // Monotypes can cause a "System.TypeLoadException: Recursive type definition detected" error from IsAssignableFrom, this bypasses it
            if (type.Name.Contains("Mono") && type.Name.Contains("Security"))
                continue;

            if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            {
                try
                {
                    runOnValid(type);
                }
                catch (Exception e)
                {
                    Core.Instance.LoggerInstance.Error(e.Message);
                }
            }
        }
    }
    
    public static byte[] LoadBytesFromAssembly(Assembly assembly, string name)
    {
        string[] manifestResources = assembly.GetManifestResourceNames();

        if (!manifestResources.Contains(name))
        {
            return null;
        }

        using Stream str = assembly.GetManifestResourceStream(name);
        using MemoryStream memoryStream = new();

        str.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }

    public static Assembly LoadAssemblyFromAssembly(Assembly assembly, string name)
    {
        var rawAssembly = LoadBytesFromAssembly(assembly, name);

        if (rawAssembly == null)
        {
            return null;
        }

        return Assembly.Load(rawAssembly);
    }
    
    public static Vector2 FlipXCoordinates(this Vector2 vector)
    {
        vector.x *= -1f;
        return vector;
    }
}