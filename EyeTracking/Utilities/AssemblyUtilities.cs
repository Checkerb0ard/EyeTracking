using System.Reflection;

namespace EyeTracking.Utilities;

internal static class AssemblyUtilities
{
    internal static void LoadAllValid<T>(Assembly assembly, Action<Type> runOnValid)
    {
        Type[] types;

        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            types = ex.Types;
        }

        foreach (Type type in types)
        {
            if (type == null)
                continue;

            if (type.Name.Contains("Mono") && type.Name.Contains("Security"))
                continue;

            bool isValid = false;

            try
            {
                isValid = typeof(T).IsAssignableFrom(type);
            }
            catch
            {
                continue;
            }

            if (isValid && !type.IsAbstract && !type.IsInterface)
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
}