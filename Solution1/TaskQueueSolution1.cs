using System.Collections.Concurrent;

namespace WebApiBackgroundServiceExample.Solution1;

public class TaskQueueSolution1 : ITaskQueueSolution1
{
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _queue = new();

    public void Clear()
    {
        _queue.Clear();
    }

    public void Enqueue(Func<CancellationToken, Task> func)
    {
        _queue.Enqueue(func);
    }

    public bool TryDequeue(out Func<CancellationToken, Task> func)
    {
        return _queue.TryDequeue(out func);
    }
}