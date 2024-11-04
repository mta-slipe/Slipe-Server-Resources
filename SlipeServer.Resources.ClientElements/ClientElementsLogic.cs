using SlipeServer.Resources.Base;
using SlipeServer.Server;

namespace SlipeServer.Resources.ClientElements;

public sealed class ClientElementsOptions : ResourceOptionsBase;

internal sealed class ClientElementsLogic : ResourceLogicBase<ClientElementsResource, ClientElementsOptions>
{
    public ClientElementsLogic(MtaServer server) : base(server)
    {
    }
}
