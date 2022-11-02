using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.NoClip;

internal class NoClipLogic
{
    private readonly MtaServer _server;
    private readonly NoClipService _noClipService;
    private readonly NoClipResource _resource;

    private readonly HashSet<Player> _noClipPlayers = new();

    public NoClipLogic(MtaServer server, NoClipService noClipService)
    {
        _server = server;
        _noClipService = noClipService;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<NoClipResource>();
        _noClipService.NoClipStateChanged += SetNoClipEnabled;
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
            player.TriggerLuaEvent("internalSetNoClipEnabled", player, enabled);
    }

    private void HandlePlayerJoin(Player player)
    {
        _resource.StartFor(player);
        var options = _resource.Options;
        player.ResourceStarted += (player, @event) =>
        {
            if (@event.NetId == _resource.NetId)
                player.TriggerLuaEvent("internalUpdateConfiguration", player, options.VerticalSpeed, options.HorizontalSpeed);
        };
        if (options.Bind != null)
        {
            player.SetBind(options.Bind, Server.Elements.Enums.KeyState.Up);
            player.BindExecuted += Player_BindExecuted;
        }
    }

    private void Player_BindExecuted(Player player, Server.Elements.Events.PlayerBindExecutedEventArgs e)
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
