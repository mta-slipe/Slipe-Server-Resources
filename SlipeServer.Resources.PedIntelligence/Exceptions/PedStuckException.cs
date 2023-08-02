using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.Exceptions;

public class PedStuckException : PedAiException
{
    public PedStuckException(Ped ped) : base(ped)
    {

    }
}
