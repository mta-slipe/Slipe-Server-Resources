using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence;

internal class PedIntelliganceState : IPedIntelliganceState
{
    public Guid Id { get; }
    public Ped Ped { get; }
    public PedTask[] Tasks { get; }
    public int TaskId { get; set; }
    public int TotalTasks => Tasks.Length;

    public event Action<IPedIntelliganceState, int>? TaskCompleted;
    public event Action<IPedIntelliganceState>? AllTasksCompleted;
    public Task Completed { get
        {
            var task = new TaskCompletionSource();
            this.AllTasksCompleted += e =>
            {
                task.SetResult();
            };

            return task.Task;
        } }

    public PedIntelliganceState(Ped ped, IEnumerable<PedTask> tasks)
    {
        Ped = ped;
        Id = Guid.NewGuid();
        Tasks = tasks.ToArray();
        TaskId = 0;
    }

    public bool AdvanceToNextTask()
    {
        TaskCompleted?.Invoke(this, TaskId++);
        TaskId++;
        if (TaskId == TotalTasks)
        {
            return true;
        }
        else
            return false;
    }

    public void Complete()
    {
        AllTasksCompleted?.Invoke(this);
    }
}
