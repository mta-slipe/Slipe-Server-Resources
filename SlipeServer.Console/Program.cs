using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Definitions.Sync;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Resources.DGS;
using SlipeServer.Resources.GuiProxy;
using SlipeServer.Resources.NoClip;
using SlipeServer.Resources.Parachute;
using SlipeServer.Server;
using SlipeServer.Server.Behaviour;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Loggers;
using SlipeServer.Server.PacketHandling.Handlers.Middleware;
using SlipeServer.Server.ServerBuilders;
using SlipeServer.Server.Services;
using System;
using System.Numerics;
using System.Threading;

namespace SlipeServer.Console;

public partial class Program
{
    public static void Main(string[] args)
    {
        Program? program = null;
        try
        {
            program = new Program(args);
            program.Start();
        }
        catch (Exception exception)
        {
            if (program != null)
            {
                program.Logger.LogCritical(exception, "{message}", exception.Message);
            }
            else
            {
                System.Console.WriteLine($"Error in startup {exception.Message}");
            }
            System.Console.WriteLine("Press any key to exit...");
            //System.Console.ReadKey();
            throw;
        }
    }

    private readonly EventWaitHandle waitHandle = new(false, EventResetMode.AutoReset);
    private readonly MtaServer server;
    private readonly Configuration configuration;

    public ILogger Logger { get; }
    public TestResource testResource;

    public Program(string[] args)
    {
        this.configuration = new Configuration()
        {
            IsVoiceEnabled = true
        };

        this.server = MtaServer.Create(
            (builder) =>
            {
                builder.UseConfiguration(this.configuration);

#if DEBUG
                builder.AddDefaults(exceptBehaviours: ServerBuilderDefaultBehaviours.MasterServerAnnouncementBehaviour);
                builder.AddNetWrapper(dllPath: "net_d", port: (ushort)(this.configuration.Port + 1));
#else
                    builder.AddDefaults();
#endif

                builder.AddBuildStep(server =>
                {
                    testResource = new TestResource(server);
                    testResource.InjectDGSExportedFunctions();
                    server.AddAdditionalResource(testResource, new());
                });

                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ILogger, ConsoleLogger>();
                });

                builder.AddNoClipResource();
                builder.AddParachuteResource();
                builder.AddDGSResource(DGSVersion.Release_3_518);
                builder.AddGuiProxy();

                builder.AddLogic<GuiProxyLogic>();
            }
        );

        this.server.GameType = "Slipe Server";
        this.server.MapName = "N/A";

        this.Logger = this.server.GetRequiredService<ILogger>();

        System.Console.CancelKeyPress += (sender, args) =>
        {
            this.server.Stop();
            this.waitHandle.Set();
        };

        this.server.PlayerJoined += Server_PlayerJoined;
    }

    private void Server_PlayerJoined(Player player)
    {
        player.Spawn(new Vector3(0, 0, 4), 0, 0, 0, 0);
        player.Camera.Target = player;
        player.Camera.Fade(CameraFade.In);
        player.AddWeapon(Server.Enums.WeaponId.Parachute, 1, true);

        var chatBox = this.server.GetRequiredService<ChatBox>();
        chatBox.OutputTo(player, "Press num_0 to enable no clip.");
        testResource.StartFor(player);
    }

    public void Start()
    {
        this.server.Start();
        this.Logger.LogInformation("Server started.");
        this.waitHandle.WaitOne();
    }
}
