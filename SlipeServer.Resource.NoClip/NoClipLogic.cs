﻿using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.NoClip;

internal class NoClipLogic
{
    private readonly MtaServer _server;
    private readonly NoClipService _noClipService;
    private readonly ILuaEventHub<INoClipEventHub> luaEventHub;
    private readonly ILogger<NoClipLogic> logger;
    private readonly NoClipResource _resource;

    private readonly HashSet<Player> _noClipPlayers = new();

    public NoClipLogic(MtaServer server, NoClipService noClipService, ILuaEventHub<INoClipEventHub> luaEventHub, ILogger<NoClipLogic> logger)
    {
        _server = server;
        _noClipService = noClipService;
        this.luaEventHub = luaEventHub;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<NoClipResource>();
        _noClipService.NoClipStateChanged += SetNoClipEnabled;
        _noClipService.PositionChanged += HandlePositionChanged;
    }

    private void HandlePositionChanged(Player player, Vector3 position)
    {
        if (_noClipPlayers.Contains(player))
            luaEventHub.Invoke(player, x => x.SetPosition(position.X, position.Y, position.Z));
    }

    private void SetNoClipEnabled(Player player, bool enabled)
    {
        if (enabled)
        {
            if (_noClipPlayers.Contains(player))
                return;
            _noClipPlayers.Add(player);
        }
        else
        {
            if (!_noClipPlayers.Contains(player))
                return;
            _noClipPlayers.Remove(player);
        }

        if (_noClipPlayers.Contains(player) == enabled)
            luaEventHub.Invoke(player, x => x.SetEnabled(enabled));
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            var options = _resource.Options;
            await _resource.StartForAsync(player);
            luaEventHub.Invoke(player, x => x.UpdateConfiguration(options.VerticalSpeed, options.HorizontalSpeed));

            if (options.Bind != null)
            {
                player.SetBind(options.Bind, Server.Elements.Enums.KeyState.Up);
                player.BindExecuted += HandleBindExecuted;
            }
        }
        catch(Exception ex)
        {
            logger.ResourceFailedToStart<NoClipResource>(ex, player);
        }
    }

    private void HandleBindExecuted(Player player, Server.Elements.Events.PlayerBindExecutedEventArgs e)
    {
        if (e.Key == _resource.Options.Bind && e.KeyState == Server.Elements.Enums.KeyState.Up)
        {
            bool isNoClipEnabled = _noClipPlayers.Contains(player);
            if (!isNoClipEnabled)
            {
                if (_resource.Options.AuthorizationCallback != null)
                {
                    if (!_resource.Options.AuthorizationCallback(player))
                        return;
                }
                SetNoClipEnabled(player, true);
            }
            else
            {
                SetNoClipEnabled(player, false);
            }
        }
    }
}
