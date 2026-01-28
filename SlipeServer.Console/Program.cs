using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Resources.BoneAttach;
using SlipeServer.Resources.DGS;
using SlipeServer.Resources.NoClip;
using SlipeServer.Resources.Parachute;
using SlipeServer.Resources.PedIntelligence;
using SlipeServer.Resources.Reload;
using SlipeServer.Resources.Text3d;
using SlipeServer.Resources.Watermark;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Loggers;
using SlipeServer.Server.ServerBuilders;
using SlipeServer.Server.Services;
using System.Drawing;
using System.Numerics;
using SlipeServer.Resources.Scoreboard;
using SlipeServer.Resources.ClientElements;
using SlipeServer.Resources.DiscordRichPresence;
using SlipeServer.Resources.Screenshots;
using SlipeServer.Resources.Base;
using SlipeServer.Resources.Assets;
using SlipeServer.Console;

EventWaitHandle waitHandle = new(false, EventResetMode.AutoReset);
Configuration configuration;

ILogger Logger;
TestResource? testResource = null;

configuration = new Configuration
{
    HttpPort = 22005,
    IsVoiceEnabled = true
};

var server = MtaServer.Create<Player>(
    (builder) =>
    {
        builder.UseConfiguration(configuration);

#if DEBUG
        builder.AddDefaults(exceptBehaviours: ServerBuilderDefaultBehaviours.MasterServerAnnouncementBehaviour);
#else
            builder.AddDefaults();
#endif

        builder.AddBuildStep(server =>
        {
            testResource = new TestResource(server);
            testResource.InjectDGSExportedFunctions();
            testResource.InjectAssetsExportedFunctions();
            server.AddAdditionalResource(testResource, []);
        });

        builder.ConfigureServices(services =>
        {
            services.AddResources(new DefaultResourcesOptions
            {
                Autostart = true
            });
            services.AddHttpClient();
            services.AddSingleton<ILogger, ConsoleLogger>();
        });

        builder.AddNoClipResource(new());
        builder.AddParachuteResource(new());
        var style = DGSStyleFactory.CreateFromColors(Color.Black, Color.Gray, Color.White);
        builder.AddDGSResource(DGSVersion.Release_3_520, style);
        builder.AddText3dResource(new());
        builder.AddReloadResource(new());
        builder.AddWatermarkResource(new());
        builder.AddPedIntelligenceResource(new());
        builder.AddScoreboard();
        builder.AddBoneAttachResource(new BoneAttachOptions
        {
            Version = BoneAttachVersion.Release_1_2_3
        });
        builder.AddClientElementsResource(new());
        builder.AddDiscordRichPresenceResource(new DiscordRichPresenceOptions
        {
            Autostart = false,
            ApplicationId = 1162033070740869120
        });
        builder.AddScreenshotsResource(new());
        builder.AddAssetsResource(new AssetsOptions
        {
            AssetsProviders = [new FileSystemAssetsProvider("Assets")]
        });
        builder.AddLogic<TestLogic>();
        builder.AddLogic<PedIntelligenceTestLogic>();
        builder.AddLogic<HotReloadableLogic>();
    }
);

server.GameType = "Slipe Server";
server.MapName = "N/A";

Logger = server.GetRequiredService<ILogger>();

System.Console.CancelKeyPress += (sender, args) =>
{
    server.Stop();
    waitHandle.Set();
};

server.PlayerJoined += HandlePlayerJoin;
server.Start();
Logger.LogInformation("Server started.");
waitHandle.WaitOne();


void HandlePlayerJoin(Player player)
{
    player.Spawn(new Vector3(0, 0, 4), 0, 0, 0, 0);
    player.Camera.Target = player;
    player.Camera.Fade(CameraFade.In);
    player.AddWeapon(SlipeServer.Server.Enums.WeaponId.Parachute, 1, true);
    player.AddWeapon(SlipeServer.Server.Enums.WeaponId.Camera, 500, true);
    player.AddWeapon(SlipeServer.Server.Enums.WeaponId.M4, 500, false);

    var chatBox = server.GetRequiredService<ChatBox>();
    chatBox.OutputTo(player, "Press num_0 to enable no clip.");
    testResource?.StartFor(player);
}
