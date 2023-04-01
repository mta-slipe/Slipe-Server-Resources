using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.Interfaces;

public interface IPedIntelliganceState
{
    Ped Ped { get; }
    int TaskId { get; }
    int TotalTasks { get; }
    Task Completed { get; }
    bool IsCompleted { get; }

    event Action<IPedIntelliganceState, int> TaskCompleted;
    event Action<IPedIntelliganceState> AllTasksCompleted;

    internal void Complete();
    internal bool AdvanceToNextTask();

    void Stop();
}
