using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.Exceptions;

public class PedAiException : Exception
{
    public Ped Ped { get; }

    public PedAiException(Ped ped)
    {
        Ped = ped;
    }
}