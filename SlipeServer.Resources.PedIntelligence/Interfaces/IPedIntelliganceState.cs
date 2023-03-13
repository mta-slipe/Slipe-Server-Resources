using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.Interfaces;

public interface IPedIntelliganceState
{
    Ped Ped { get; }
    int TaskId { get; }
    int TotalTasks { get; }
    Task Completed { get; }

    event Action<IPedIntelliganceState, int> TaskCompleted;
    event Action<IPedIntelliganceState> AllTasksCompleted;

    internal void Complete();
    internal bool AdvanceToNextTask();
}
