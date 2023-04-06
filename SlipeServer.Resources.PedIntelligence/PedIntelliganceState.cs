using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.PedIntelligence;

internal class PedIntelliganceState : IPedIntelliganceState
{
    public Guid Id { get; }
    public Ped Ped { get; }
    public PedTask[] Tasks { get; }
    public int TaskId { get; set; }
    public int TotalTasks => Tasks.Length;

    public bool IsCompleted => TaskId == TotalTasks;

    public event Action<IPedIntelliganceState, int>? TaskCompleted;
    public event Action<IPedIntelliganceState>? AllTasksCompleted;
    public event Action<IPedIntelliganceState>? Stopped;
    public Task Completed { get
        {
            var task = new TaskCompletionSource();
            void HandleCompleted(IPedIntelliganceState e)
            {
                task.SetResult();
                this.AllTasksCompleted -= HandleCompleted;
                this.Stopped -= HandleStopped;
            };

            void HandleStopped(IPedIntelliganceState e)
            {
                task.SetException(new OperationCanceledException());
                this.AllTasksCompleted -= HandleCompleted;
                this.Stopped -= HandleStopped;
            };

            this.AllTasksCompleted += HandleCompleted;
            this.Stopped += HandleStopped;

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
        if (IsCompleted)
            throw new InvalidOperationException();

        AllTasksCompleted?.Invoke(this);
    }

    public void Stop()
    {
        if (IsCompleted)
            throw new InvalidOperationException();

        Stopped?.Invoke(this);
    }
}
