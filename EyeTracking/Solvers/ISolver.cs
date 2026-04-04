using Il2CppSLZ.VRMK;

namespace EyeTracking.Solvers;

internal interface ISolver
{
    internal abstract void OnAvatarSwitch(Avatar avatar);
    internal abstract void Update();
}