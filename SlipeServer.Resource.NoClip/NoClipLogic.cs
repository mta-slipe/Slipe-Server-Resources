using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.NoClip;

internal sealed class NoClipLogic : ResourceLogicBase<NoClipResource, NoClipOptions>
{
    private readonly NoClipService noClipService;
    private readonly ILuaEventHub<INoClipEventHub> luaEventHub;

    private readonly HashSet<Player> _noClipPlayers = new();

    public NoClipLogic(MtaServer server, NoClipService noClipService, ILuaEventHub<INoClipEventHub> luaEventHub) : base(server)
    {
        this.noClipService = noClipService;
        this.luaEventHub = luaEventHub;

        this.noClipService.NoClipStateChanged += SetNoClipEnabled;
        this.noClipService.PositionChanged += HandlePositionChanged;
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

    protected override void HandleResourceStarted(Player player)
    {
        luaEventHub.Invoke(player, x => x.UpdateConfiguration(this.resource.Options.VerticalSpeed, this.resource.Options.HorizontalSpeed));

        if (this.resource.Options.Bind != null)
        {
            player.SetBind(this.resource.Options.Bind, Server.Elements.Enums.KeyState.Up);
            player.BindExecuted += HandleBindExecuted;
        }
    }

    private void HandleBindExecuted(Player player, Server.Elements.Events.PlayerBindExecutedEventArgs e)
    {
        if (e.Key == this.resource.Options.Bind && e.KeyState == Server.Elements.Enums.KeyState.Up)
        {
            bool isNoClipEnabled = _noClipPlayers.Contains(player);
            if (!isNoClipEnabled)
            {
                if (this.resource.Options.AuthorizationCallback != null)
                {
                    if (!this.resource.Options.AuthorizationCallback(player))
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
