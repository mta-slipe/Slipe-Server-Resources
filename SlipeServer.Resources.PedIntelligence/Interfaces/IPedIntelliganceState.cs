using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.Interfaces;

public interface IPedIntelligenceState
{
    Ped Ped { get; }
    int TaskId { get; }
    int TotalTasks { get; }
    Task Completed { get; }
    bool IsCompleted { get; }

    event Action<IPedIntelligenceState, int> TaskCompleted;
    event Action<IPedIntelligenceState> AllTasksCompleted;

    internal void Complete();
    internal bool AdvanceToNextTask();

    void Stop(Exception? ex);
}
