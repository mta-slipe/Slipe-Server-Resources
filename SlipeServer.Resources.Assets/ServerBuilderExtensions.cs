using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Server.Resources;
using SlipeServer.Server.ServerBuilders;
using System.Text;

namespace SlipeServer.Resources.Assets;

public abstract record AssetsProvider;
public sealed record FileSystemAssetsProvider(string Path) : AssetsProvider;

public sealed class AssetsOptions : ResourceOptionsBase
{
    public string BasePath { get; set; } = "Data";
    public AssetsProvider[] AssetsProviders { get; set; } = [];
}

public static class ServerBuilderExtensions
{
    public static void AddAssetsResource(this ServerBuilder builder, AssetsOptions assetsOptions)
    {
        assetsOptions ??= new();
        builder.AddBuildStep(server =>
        {
            var resource = new AssetsResource(server);

            var additionalFiles = resource.GetAndAddLuaFiles();
            foreach (var assetsProvider in assetsOptions.AssetsProviders)
            {
                switch (assetsProvider)
                {
                    case FileSystemAssetsProvider fileSystemAssetsProvider:
                        foreach (var pair in resource.AddAssetsFromFileSystem(fileSystemAssetsProvider.Path, assetsOptions.BasePath))
                        {
                            additionalFiles[pair.Item1] = pair.Item2;
                        }
                        break;
                }
            }

            server.AddAdditionalResource(resource, additionalFiles);
            resource.AddLuaEventHub<IAsssetsEventHub>();
        });

        builder.ConfigureServices(services =>
        {
            services.AddAssetsServices(assetsOptions);
        });

        builder.AddLogic<AssetsLogic>();
    }

    public static IServiceCollection AddAssetsServices(this IServiceCollection services, AssetsOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddLuaEventHub<IAsssetsEventHub, AssetsResource>();
        services.AddSingleton<AssetsService>();
        services.AddSingleton<AssetsManager>();
        return services;
    }
}


public static class ResourceExtensions
{
    internal static IEnumerable<(string, byte[])> AddAssetsFromFileSystem(this Resource resource, string path, string basePath)
    {
        var files = Directory.GetFiles(path);
        foreach (var fileName in files)
        {
            var content = File.ReadAllBytes(fileName);
            var relativeFileName = Path.GetRelativePath(path, fileName);
            resource.Files.Add(ResourceFileFactory.FromBytes(content, Path.Combine(basePath, relativeFileName)));
            yield return (Path.Join(basePath, relativeFileName), content);
        }
    }

    public static void InjectAssetsExportedFunctions(this Resource resource)
    {
        resource.NoClientScripts[$"{resource.Name}/assetsExports.lua"] =
            Encoding.UTF8.GetBytes("""
                local cache = {}
                function assetsGetRawData(assetSource)
                    local asset = exports.Assets:assetsGetRawData(assetSource);
                    return asset;
                end

                local function assetsLoad(assetSource, factory)
                    local data = assetsGetRawData(assetSource)
                    
                    if(data.error)then
                        return {
                            isLoaded = true,
                            error = data.error
                        };
                    end
                    if(data.hasValue)then
                        local success, assetOrError = pcall(factory, data.value)
                
                        if success then
                            return {
                                value = assetOrError,
                                isLoaded = true,
                            }
                        else
                            return {
                                error = assetOrError,
                                isLoaded = false,
                            }
                        end
                    else
                        local result = {
                            isLoaded = false,
                        }
                
                        setTimer(function()
                            local data = assetsGetRawData(assetSource)
                            if(data.error)then
                                killTimer(sourceTimer);
                                result.error = data.error;
                            elseif(data.hasValue)then
                                killTimer(sourceTimer);
                                local success, assetOrError = pcall(factory, data.value)
                
                                if success then
                                    result.value = assetOrError;
                                else
                                    result.error = assetOrError;
                                end
                            end
                        end, 200, 0);

                        return result;
                    end
                
                    return {
                        isLoaded = false,
                        error = "unknown"
                    }
                end

                function assetsGetImage(assetSource)
                    return assetsLoad(assetSource, dxCreateTexture);
                end
                """);
    }
}
