using System.Collections.Concurrent;

namespace WebApiBackgroundServiceExample.Solution2;

public class TaskQueueSolution2 : ITaskQueueSolution2
{
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _queue = new();
    private readonly SemaphoreSlim _signal = new(0);

    public void Clear()
    {
        _queue.Clear();
    }

    public async Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _queue.TryDequeue(out var workItem);

        return workItem!;
    }

    public void Enqueue(Func<CancellationToken, Task> item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _queue.Enqueue(item);
        _signal.Release();
    }
}