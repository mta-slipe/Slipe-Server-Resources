using SlipeServer.Packets.Enums;
using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.BoneAttach;

public class BoneAttachService
{
    internal Action<Element, Ped, BoneId, Vector3?, Vector3?, bool>? Attached;
    internal Action<Element>? Detached;
    internal Action<Ped>? DetachedAll;
    internal Action<Element, Vector3>? PositionOffsetChanged;
    internal Action<Element, Vector3>? RotationOffsetChanged;
    internal Action<Element, Ped>? ElementPedChanged;
    internal Action<Element, BoneId>? BoneChanged;

    internal Func<Element, bool>? InternalIsAttached;
    internal Func<Element, AttachInfo>? InternalGetAttachInfo;
    internal Func<Ped, List<Element>>? InternalGetAttacheds;

    public event Action<Ped, Element>? ElementDetached;

    internal void RelayElementDetached(Ped ped, Element element)
    {
        ElementDetached?.Invoke(ped, element);
    }

    public void Attach(Element element, Ped ped, BoneId boneId, Vector3? positionOffset = null, Vector3? rotationOffset = null, bool toggleCollision = true)
    {
        Attached?.Invoke(element, ped, boneId, positionOffset, rotationOffset, toggleCollision);
    }

    public void Detach(Element element)
    {
        Detached?.Invoke(element);
    }

    public void DetachAll(Ped ped)
    {
        DetachedAll?.Invoke(ped);
    }

    public void SetPositionOffset(Element element, Vector3 positionOffset)
    {
        PositionOffsetChanged?.Invoke(element, positionOffset);
    }

    public void SetRotationOffset(Element element, Vector3 rotationOffset)
    {
        RotationOffsetChanged?.Invoke(element, rotationOffset);
    }

    public void SetPed(Element element, Ped ped)
    {
        ElementPedChanged?.Invoke(element, ped);
    }

    public void SetBone(Element element, BoneId boneId)
    {
        BoneChanged?.Invoke(element, boneId);
    }

    public bool IsAttached(Element element) => InternalIsAttached?.Invoke(element) ?? throw new InvalidOperationException();

    public AttachInfo GetAttachInfo(Element element) => InternalGetAttachInfo?.Invoke(element) ?? throw new InvalidOperationException();

    public List<Element> GetAttacheds(Ped ped) => InternalGetAttacheds?.Invoke(ped) ?? throw new InvalidOperationException();
}
