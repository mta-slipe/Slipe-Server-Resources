using Microsoft.Extensions.Logging;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Base;

public static class LoggerExtensions
{
    public static void ResourceFailedToStart<T>(this ILogger logger, Exception ex, Player player) where T : Resource
    {
        logger.LogTrace(ex, "Failed to start resource '{resourceType}' for player {playerName}", typeof(T).Name, player.Name);
    }
}