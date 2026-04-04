using EyeTracking.BoneMenu;

namespace EyeTracking.Solvers;

internal class SolverManager
{
    internal EyeSolver EyeSolver { get; private set; }
    internal FaceSolver FaceSolver { get; private set; }

    internal SolverManager()
    {
        EyeSolver = new EyeSolver();
        FaceSolver = new FaceSolver();
    }

    internal void Update()
    {
        EyeSolver.Update();
        FaceSolver.Update();
    }
}