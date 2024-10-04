using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence;

internal class PedIntelligenceState : IPedIntelligenceState
{
    public Guid Id { get; }
    public Ped Ped { get; }
    public PedTask[] Tasks { get; }
    public int TaskId { get; set; }
    public int TotalTasks => Tasks.Length;

    public bool IsCompleted => TaskId == TotalTasks;

    public event Action<IPedIntelligenceState, int>? TaskCompleted;
    public event Action<IPedIntelligenceState>? AllTasksCompleted;
    public event Action<IPedIntelligenceState, Exception?>? Stopped;
    public Task Completed
    {
        get
        {
            var task = new TaskCompletionSource();
            void HandleCompleted(IPedIntelligenceState e)
            {
                task.SetResult();
                this.AllTasksCompleted -= HandleCompleted;
                this.Stopped -= HandleStopped;
            };

            void HandleStopped(IPedIntelligenceState e, Exception? ex)
            {
                task.SetException(ex ?? new OperationCanceledException());
                this.AllTasksCompleted -= HandleCompleted;
                this.Stopped -= HandleStopped;
            };

            this.AllTasksCompleted += HandleCompleted;
            this.Stopped += HandleStopped;

            return task.Task;
        }
    }

    public PedIntelligenceState(Ped ped, IEnumerable<PedTask> tasks)
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
        TaskId = TotalTasks;
        AllTasksCompleted?.Invoke(this);
    }

    public void Stop(Exception? ex = null)
    {
        if (IsCompleted)
            throw new InvalidOperationException();

        Stopped?.Invoke(this, ex);
    }
}
