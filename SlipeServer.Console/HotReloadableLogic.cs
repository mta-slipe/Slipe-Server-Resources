using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Console;

internal class HotResource : Resource
{
    internal HotResource(MtaServer server)
        : base(server, server.RootElement, "Hot")
    {
    }
}

internal class HotReloadableLogic
{
    private readonly IElementCollection elementCollection;
    private readonly MtaServer mtaServer;
    private readonly FileSystemWatcher watcher;
    private HotResource hotResource;
    private readonly string projectDirectory;

    public HotReloadableLogic(IElementCollection elementCollection, MtaServer mtaServer)
    {
        this.elementCollection = elementCollection;
        this.mtaServer = mtaServer;

        string outputDirectory = AppDomain.CurrentDomain.BaseDirectory;
        this.projectDirectory = Directory.GetParent(outputDirectory).Parent.Parent.Parent.FullName; // Navigate to the project folder

        this.watcher = new()
        {
            Filter = "*.lua",

            Path = this.projectDirectory,
            NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.Size
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
        };

        this.watcher.Changed += HandleChanged;
        this.watcher.EnableRaisingEvents = true;

        this.hotResource = this.CreateHotResource();

        mtaServer.PlayerJoined += HandlePlayerJoined;
    }

    private HotResource CreateHotResource()
    {
        var resource = new HotResource(this.mtaServer);
        var bytes = File.ReadAllBytes(Path.Join(this.projectDirectory, "HotReloadable.lua"));
        resource.AddNoClientScript("HotReloadable.lua", bytes);
        this.mtaServer.AddAdditionalResource(resource, []);
        return resource;
    }

    private void HandlePlayerJoined(Player player)
    {
        this.hotResource.StartFor(player);
    }

    private void HandleChanged(object sender, FileSystemEventArgs e)
    {
        var players = this.elementCollection.GetByType<Player>();

        foreach (var player in players)
            this.hotResource.StopFor(player);

        this.mtaServer.RemoveAdditionalResource(this.hotResource);
        this.hotResource = this.CreateHotResource();

        foreach (var player in players)
            this.hotResource.StartFor(player);

        System.Console.WriteLine("HotReloadble.lua reloaded");
    }
}
