﻿using SlipeServer.Server.Elements;
using System;

namespace SlipeServer.Resource.NoClip;

public class NoClipService
{
    internal event Action<Player, bool>? NoClipStateChanged;

    public void SetEnabledTo(Player player, bool enabled)
    {
        NoClipStateChanged?.Invoke(player, enabled);
    }
}
