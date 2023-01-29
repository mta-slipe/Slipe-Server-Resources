using SlipeServer.Packets.Enums;
using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.BoneAttach;

public struct AttachInfo
{
    public Ped ped;
    public BoneId boneId;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
}
