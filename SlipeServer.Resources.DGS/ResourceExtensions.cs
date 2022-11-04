using SlipeServer.Server.Resources;
using System.Text;

namespace SlipeServer.Resources.DGS;

public static class ResourceExtensions
{
    public static void InjectDGSExportedFunctions(this Resource resource)
    {
        resource.NoClientScripts[$"{resource.Name}/dgsExports.lua"] =
            Encoding.UTF8.GetBytes("loadstring(exports.dgs:dgsImportFunction())()");
    }
}
