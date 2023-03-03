namespace SlipeServer.Resources.DGS;

public class IndentScope : IDisposable
{
    public static int CurrentIndent { get; private set; } = 0;
    public IndentScope()
    {
        CurrentIndent++;
    }

    public void Dispose()
    {
        CurrentIndent--;
    }
}
