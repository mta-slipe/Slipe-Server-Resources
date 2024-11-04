using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Packets.Enums;
using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System.Numerics;

namespace SlipeServer.Resources.BoneAttach;

internal sealed class BoneAttachLogic : ResourceLogicBase<BoneAttachResource, BoneAttachOptions>
{
    private readonly BoneAttachService boneAttachService;
    private readonly LuaEventService luaEventService;

    private readonly Dictionary<Element, AttachInfo> cache = new();
    private readonly Dictionary<string, LuaValue> _options = new()
    {
        ["dimensionChanges"] = true,
        ["interiorChanges"] = true,
    };
    private readonly object cacheLock = new();

    public BoneAttachLogic(MtaServer server, BoneAttachService boneAttachService, LuaEventService luaEventService) : base(server)
    {
        this.boneAttachService = boneAttachService;
        this.luaEventService = luaEventService;

        this.boneAttachService.Attached = HandleAttached;
        this.boneAttachService.Detached = HandleDetached;
        this.boneAttachService.DetachedAll = HandleDetachAll;
        this.boneAttachService.PositionOffsetChanged = HandleSetPositionOffsetChanged;
        this.boneAttachService.RotationOffsetChanged = HandleSetRotationOffsetChanged;
        this.boneAttachService.ElementPedChanged = HandleSetPed;
        this.boneAttachService.BoneChanged = HandleSetBone;
        this.boneAttachService.OptionsChanged = HandleOptionsChanged;
        this.boneAttachService.InternalIsAttached = IsAttached;
        this.boneAttachService.InternalGetAttachInfo = GetAttachInfo;
        this.boneAttachService.InternalGetAttacheds = GetAttacheds;
    }

    private void SendCacheToPlayer(Player player)
    {
        LuaValue[] luaValues = new LuaValue[cache.Count];
        int i = 0;
        lock (cacheLock)
        {
            foreach (var keyValuePair in cache)
            {
                LuaValue[] row = new LuaValue[9];
                var po = keyValuePair.Value.positionOffset;
                var ro = keyValuePair.Value.rotationOffset;
                row[0] = keyValuePair.Key.Id;
                row[1] = keyValuePair.Value.ped.Id;
                row[2] = (int)keyValuePair.Value.boneId;
                row[3] = po.X;
                row[4] = po.Y;
                row[5] = po.Z;
                row[6] = ro.X;
                row[7] = ro.Y;
                row[8] = ro.Z;
                luaValues[i++] = row;
            }
        }
        luaEventService.TriggerEventFor(player, "pAttach:receiveCache", resource.Root, new LuaValue(luaValues), _options);
    }

    private void HandlePedDestroyed(Element ped)
    {
        HandleDetachAll((Ped)ped);
    }

    private void HandleElementDestroyed(Element element)
    {
        HandleDetached(element);
    }

    private void HandleAttached(Element element, Ped ped, BoneId boneId, Vector3? positionOffset, Vector3? rotationOffset, bool toggleCollision = true)
    {
        var po = positionOffset.GetValueOrDefault();
        var ro = rotationOffset.GetValueOrDefault();

        lock (cacheLock)
        {
            if (!cache.ContainsKey(element))
                cache[element] = new AttachInfo
                {
                    boneId = boneId,
                    ped = ped,
                    positionOffset = po,
                    rotationOffset = ro,
                };
            else
                throw new Exception("Element already attached.");
        }

        element.Destroyed += HandleElementDestroyed;
        ped.Destroyed += HandlePedDestroyed;
        luaEventService.TriggerEvent("pAttach:attach", resource.Root, element, ped, (int)boneId, po.X, po.Y, po.Z, ro.X, ro.Y, ro.Z);
    }

    private void HandleDetached(Element element)
    {
        bool exist = false;
        Ped? ped = null;
        lock (cacheLock)
        {
            exist = cache.Remove(element, out var value);
            ped = value.ped;
        }
        if (!exist)
            throw new Exception("Element is not attached to any ped.");

        element.Destroyed -= HandleElementDestroyed;
        boneAttachService.RelayElementDetached(ped, element);
        luaEventService.TriggerEvent("pAttach:detach", resource.Root, element, true);
    }

    private void HandleDetachAll(Ped ped)
    {
        bool anyRemoved = false;
        lock (cacheLock)
        {
            foreach (var item in cache)
            {
                if (item.Value.ped == ped)
                {
                    anyRemoved = true;
                    cache.Remove(item.Key);
                    item.Key.Destroyed -= HandleElementDestroyed;
                    boneAttachService.RelayElementDetached(ped, item.Key);
                }
            }
        }

        if (anyRemoved)
        {
            ped.Destroyed += HandlePedDestroyed;
            luaEventService.TriggerEvent("pAttach:detachAll", resource.Root, ped);
        }
    }

    private void HandleSetPositionOffsetChanged(Element element, Vector3 positionOffset)
    {
        lock (cacheLock)
        {
            if (cache.TryGetValue(element, out var attachInfo))
            {
                attachInfo.positionOffset = positionOffset;
                cache[element] = attachInfo;
            }
            else
                throw new Exception("Element is not attached to any ped.");
        }

        luaEventService.TriggerEvent("pAttach:setPositionOffset", resource.Root, element, positionOffset.X, positionOffset.Y, positionOffset.Z);
    }

    private void HandleSetRotationOffsetChanged(Element element, Vector3 rotationOffset)
    {
        lock (cacheLock)
        {
            if (cache.TryGetValue(element, out var attachInfo))
            {
                attachInfo.rotationOffset = rotationOffset;
                cache[element] = attachInfo;
            }
            else
                throw new Exception("Element is not attached to any ped.");
        }

        luaEventService.TriggerEvent("pAttach:setRotationOffset", resource.Root, element, rotationOffset.X, rotationOffset.Y, rotationOffset.Z);
    }

    private void HandleSetPed(Element element, Ped ped)
    {
        lock (cacheLock)
        {
            if (cache.TryGetValue(element, out var attachInfo))
            {
                attachInfo.ped.Destroyed -= HandlePedDestroyed;
                attachInfo.ped = ped;
                attachInfo.ped.Destroyed += HandlePedDestroyed;
                cache[element] = attachInfo;
            }
            else
                throw new Exception("Element is not attached to any ped.");
        }

        luaEventService.TriggerEvent("pAttach:setPed", resource.Root, element, ped);
    }

    private void HandleOptionsChanged(string option, LuaValue value)
    {
        _options[option] = value;
        luaEventService.TriggerEvent("pAttach:setConfigOption", resource.Root, option, value);
    }

    private void HandleSetBone(Element element, BoneId boneId)
    {
        lock (cacheLock)
        {
            if (cache.TryGetValue(element, out var attachInfo))
            {
                attachInfo.boneId = boneId;
                cache[element] = attachInfo;
            }
            else
                throw new Exception("Element is not attached to any ped.");
        }

        luaEventService.TriggerEvent("pAttach:setBone", resource.Root, element, (int)boneId);
    }

    private bool IsAttached(Element element)
    {
        lock (cacheLock)
            return cache.ContainsKey(element);
    }

    private AttachInfo GetAttachInfo(Element element)
    {
        lock (cacheLock)
            return cache[element];
    }

    private List<Element> GetAttacheds(Ped ped)
    {
        lock (cacheLock)
            return cache
                .Where(x => x.Value.ped == ped)
                .Select(x => x.Key)
                .ToList();
    }

    protected override void HandleResourceStarted(Player player)
    {
        SendCacheToPlayer(player);
    }
}
