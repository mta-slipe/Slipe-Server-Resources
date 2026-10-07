using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.DGS;

public static class ResourceExtensions
{
    public static void InjectDGSExportedFunctions(this Resource resource)
    {
        resource.AddNoClientScript($"{resource.Name}/dgsExports.lua", "loadstring(exports.dgs:dgsImportFunction())()");
    }
}
