namespace Cezzi.Applications.Extensions;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public static class TaskExtensions
{
    /// <summary>Adds the task.</summary>
    /// <typeparam name="TTask">The type of the task.</typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <param name="task">The task.</param>
    /// <returns></returns>
    public static TTask AddTask<TTask>(this IList<Task> tasks, TTask task) where TTask : Task
    {
        if (tasks == null)
        {
            return default;
        }

        tasks.Add(task);
        return task;
    }
}
