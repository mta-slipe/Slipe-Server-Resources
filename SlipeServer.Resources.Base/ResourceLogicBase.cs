using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using SlipeServer.Server;

namespace SlipeServer.Resources.Base;

public abstract class ResourceLogicBase<TResource, TOptions> where TResource : Resource where TOptions : ResourceOptionsBase
{
    protected readonly MtaServer server;
    protected readonly ILogger logger;
    protected readonly IOptions<TOptions> resourceOptions;
    protected readonly IOptions<DefaultResourcesOptions> defaultResourcesOptions;
    protected readonly TResource resource;
    protected readonly ResourceStartedManager resourceStartedManager;

    public ResourceLogicBase(MtaServer server, ILogger logger, IOptions<TOptions> resourceOptions, IOptions<DefaultResourcesOptions> defaultResourcesOptions, ResourceStartedManager? resourceStartedManager = null)
    {
        this.resourceStartedManager = resourceStartedManager ?? throw new InvalidOperationException("ResourceStartedManager is not initialized. Please ensure that you have called services.AddResources() in your service configuration.");
        this.server = server;
        this.logger = logger;
        this.resourceOptions = resourceOptions;
        this.defaultResourcesOptions = defaultResourcesOptions;
        this.resource = this.server.GetAdditionalResource<TResource>();

        this.server.PlayerJoined += HandlePlayerJoin;
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            if (!this.resourceOptions.Value.Autostart ?? this.defaultResourcesOptions.Value.Autostart)
            {
                void handleResourceStarted(Player sender, PlayerResourceStartedEventArgs args)
                {
                    if (args.NetId != resource.NetId)
                        return;

                    try
                    {
                        HandleResourceStartedInternal(sender);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Failed to start resource {resourceName} for player {playerName}.", typeof(TResource).Name, sender.Name);
                    }
                    finally
                    {
                        player.ResourceStarted -= handleResourceStarted;
                    }
                }
                player.ResourceStarted += handleResourceStarted;
            }
            else
            {
                await resource.StartForAsync(player);
                this.HandleResourceStartedInternal(player);
            }
        }
        catch (Exception ex)
        {
            this.logger.ResourceFailedToStart<TResource>(ex, player);
        }
    }

    private void HandleResourceStartedInternal(Player player)
    {
        if (this.resourceStartedManager.Started<TResource>(player))
        {
            HandleResourceStarted(player);
        }
    }

    protected virtual void HandleResourceStarted(Player player) { }
}
