using Microsoft.Extensions.Options;
using SlipeServer.Packets.Definitions.Lua;
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
    internal Action<string, LuaValue>? OptionsChanged;

    internal Func<Element, bool>? InternalIsAttached;
    internal Func<Element, AttachInfo>? InternalGetAttachInfo;
    internal Func<Ped, List<Element>>? InternalGetAttacheds;
    private readonly IOptions<BoneAttachOptions> boneAttachOptions;

    public event Action<Ped, Element>? ElementDetached;

    public BoneAttachService(IOptions<BoneAttachOptions> boneAttachOptions)
    {
        this.boneAttachOptions = boneAttachOptions;
    }

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

    public void ToggleCollisions(bool toggle)
    {
        if(boneAttachOptions.Value.Version >= BoneAttachVersion.Release_1_2_3)
        {
            OptionsChanged?.Invoke("toggleCollisions", toggle);
        }
        else
        {
            throw new NotSupportedException("ToggleCollisions require minimmum version Release_1_2_3");
        }
    }

    public bool IsAttached(Element element) => InternalIsAttached?.Invoke(element) ?? throw new InvalidOperationException();

    public AttachInfo GetAttachInfo(Element element) => InternalGetAttachInfo?.Invoke(element) ?? throw new InvalidOperationException();

    public List<Element> GetAttacheds(Ped ped) => InternalGetAttacheds?.Invoke(ped) ?? throw new InvalidOperationException();
}
