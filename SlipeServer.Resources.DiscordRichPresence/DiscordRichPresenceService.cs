using SlipeServer.Resources.Base;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using System.Collections.Concurrent;

namespace SlipeServer.Resources.DiscordRichPresence;

public class DiscordRichPresenceService
{
    private readonly ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub;
    private readonly ConcurrentDictionary<Player, bool> playersRichPresence = new();
    public event Action<Player, string?>? RichPresenceReady;

    public DiscordRichPresenceService(ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub)
    {
        this.luaEventHub = luaEventHub;
    }

    internal void AddPlayer(Player player, bool canUseRichPresence, string? userId)
    {
        if (player.Client.IsConnected)
        {
            if(playersRichPresence.TryRemove(player, out var _))
                player.Disconnected -= HandleDisconnected;

            if (playersRichPresence.TryAdd(player, canUseRichPresence))
            {
                player.Disconnected += HandleDisconnected;
                if(canUseRichPresence)
                    RichPresenceReady?.Invoke(player, userId);
            }
        }
    }

    private void HandleDisconnected(Player player, PlayerQuitEventArgs e)
    {
        if (playersRichPresence.TryRemove(player, out var _))
            player.Disconnected -= HandleDisconnected;
    }

    private void ValidatePlayer(Player player)
    {
        if (!playersRichPresence.TryGetValue(player, out var canUseRichPresence))
            throw new DiscordRichPresenceNotReady();
        if (!canUseRichPresence)
            throw new DiscordRichPresenceNotAllowed();
    }
    
    public bool IsRichPresenceAllowed(Player player)
    {
        if (!playersRichPresence.TryGetValue(player, out var canUseRichPresence))
            return false;
        return canUseRichPresence;
    }

    public void SetState(Player player, string state)
    {
        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.SetState(state));
    }

    /// <summary>
    /// Set state for all players
    /// </summary>
    /// <param name="state"></param>
    public void SetState(string state)
    {
        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(pair.Key, x => x.SetState(state));
        }
    }

    public void SetDetails(Player player, string details)
    {
        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.SetDetails(details));
    }

    /// <summary>
    /// Set details for all players
    /// </summary>
    /// <param name="details"></param>
    public void SetDetails(string details)
    {
        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(pair.Key, x => x.SetDetails(details));
        }
    }

    public void SetAsset(Player player, string asset, string assetName)
    {
        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.SetAsset(asset, assetName));
    }

    /// <summary>
    /// Set asset for all players
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="assetName"></param>
    public void SetAsset(string asset, string assetName)
    {
        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(player, x => x.SetAsset(asset, assetName));
        }
    }

    public void SetSmallAsset(Player player, string asset, string assetName)
    {
        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.SetSmallAsset(asset, assetName));
    }

    /// <summary>
    /// Set small asset for all players
    /// </summary>
    /// <param name="player"></param>
    /// <param name="asset"></param>
    /// <param name="assetName"></param>
    public void SetSmallAsset(string asset, string assetName)
    {
        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(player, x => x.SetSmallAsset(asset, assetName));
        }
    }

    public void SetButton(Player player, DiscordRichPresenceButton discordRichPresenceButton, string text, Uri uri)
    {
        ValidatePlayer(player);
        var index = (int)discordRichPresenceButton;
        var url = uri.ToString();
        luaEventHub.Invoke(player, x => x.SetButton(index, text, url));
    }

    /// <summary>
    /// Set button for all players
    /// </summary>
    /// <param name="player"></param>
    /// <param name="discordRichPresenceButton"></param>
    /// <param name="text"></param>
    /// <param name="uri"></param>
    public void SetButton(DiscordRichPresenceButton discordRichPresenceButton, string text, Uri uri)
    {
        var index = (int)discordRichPresenceButton;
        var url = uri.ToString();
        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(player, x => x.SetButton(index, text, url));
        }
    }

    public void SetPartySize(Player player, int size, int max)
    {
        if (size > max)
            throw new ArgumentException(nameof(size));

        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.SetPartySize(size, max));
    }

    /// <summary>
    /// Set party size fo all players
    /// </summary>
    /// <param name="size"></param>
    /// <param name="max"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetPartySize(int size, int max)
    {
        if (size > max)
            throw new ArgumentException(nameof(size));

        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(player, x => x.SetPartySize(size, max));
        }
    }

    public void SetStartTime(Player player, int seconds)
    {
        if (seconds < 0)
            throw new ArgumentException(nameof(seconds));

        ValidatePlayer(player);
        luaEventHub.Invoke(player, x => x.StartTime(seconds));
    }

    /// <summary>
    /// Set start time for all players
    /// </summary>
    /// <param name="seconds"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetStartTime(int seconds)
    {
        if (seconds < 0)
            throw new ArgumentException(nameof(seconds));

        foreach (var pair in playersRichPresence.Where(x => x.Value))
        {
            var player = pair.Key;
            luaEventHub.Invoke(player, x => x.StartTime(seconds));
        }
    }
}
