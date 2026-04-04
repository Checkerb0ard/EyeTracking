using LabFusion.Network.Serialization;

namespace EyeTracking.Fusion.Messages;

internal class EyeData : INetSerializable
{
    internal float GazeX;
    internal float GazeY;
    internal float LeftOpenness;
    internal float RightOpenness;

    public int? GetSize() => sizeof(float) * 4;

    public void Serialize(INetSerializer serializer)
    {
        serializer.SerializeValue(ref GazeX);
        serializer.SerializeValue(ref GazeY);
        serializer.SerializeValue(ref LeftOpenness);
        serializer.SerializeValue(ref RightOpenness);
    }
}