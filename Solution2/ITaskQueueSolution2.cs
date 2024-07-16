namespace WebApiBackgroundServiceExample.Solution2;

public interface ITaskQueueSolution2
{
    void Clear();
    Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken cancellationToken);
    void Enqueue(Func<CancellationToken, Task> item);
}