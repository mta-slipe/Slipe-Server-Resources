namespace SlipeServer.Resources.Base;

public abstract class ResourceOptionsBase
{
    /// <summary>
    /// Gets or sets a value indicating whether to automatically start the resource when a player joins.
    /// A value of <c>true</c> enables autostart, <c>false</c> disables it, and <c>null</c> indicates that 
    /// the default configuration should be used.
    /// </summary>
    public bool? Autostart { get; set; }
}

public class DefaultResourcesOptions
{
    /// <summary>
    /// Gets or sets the default autostart configuration for the resource.
    /// If no specific configuration is provided, this value will determine whether the resource starts
    /// automatically upon player join. The default value is <c>true</c>.
    /// </summary>
    public bool Autostart { get; set; } = true;
}