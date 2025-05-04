using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Represents an asynchronous operation.
    /// </summary>
    public static class TaskEx
    {
#if !NET45_OR_GREATER
        private const string ArgumentOutOfRange_TimeoutNonNegativeOrMinusOne = "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.";
#endif

#if !NET46_OR_GREATER
        /// <summary>
        /// An already completed task.
        /// </summary>
        private static readonly Task<bool> s_preCompletedTask =
#if NET45_OR_GREATER
            Task.FromResult(false);
#else
            FromResult(false);
#endif
#endif

        /// <summary>
        /// The extension for <see cref="Task"/> class.
        /// </summary>
        extension(Task)
        {
            /// <summary>
            /// Gets a task that's already been completed successfully.
            /// </summary>
            public static Task CompletedTask =>
#if NET46_OR_GREATER
                Task.CompletedTask;
#else
                s_preCompletedTask;
#endif

            /// <summary>
            /// Creates a task that runs the specified action.
            /// </summary>
            /// <param name="action">The action to execute asynchronously.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
            public static Task Run(Action action) =>
#if NET45_OR_GREATER
                Task.Run(action);
#else
                Run(action, CancellationToken.None);
#endif

            /// <summary>
            /// Creates a task that runs the specified action.
            /// </summary>
            /// <param name="action">The action to execute.</param>
            /// <param name="cancellationToken">The CancellationToken to use to request cancellation of this task.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
            public static Task Run(Action action, CancellationToken cancellationToken) =>
#if NET45_OR_GREATER
                Task.Run(action, cancellationToken);
#else
                Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The function to execute asynchronously.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task<TResult> Run<TResult>(Func<TResult> function) =>
#if NET45_OR_GREATER
                Task.Run(function);
#else
                Run(function, CancellationToken.None);
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The action to execute.</param>
            /// <param name="cancellationToken">The CancellationToken to use to cancel the task.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken) =>
#if NET45_OR_GREATER
                Task.Run(function, cancellationToken);
#else
                Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The action to execute asynchronously.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task Run(Func<Task> function) =>
#if NET45_OR_GREATER
                Task.Run(function);
#else
                Run(function, CancellationToken.None);
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The function to execute.</param>
            /// <param name="cancellationToken">The CancellationToken to use to request cancellation of this task.</param>
            /// <returns>A task that represents the completion of the function.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task Run(Func<Task> function, CancellationToken cancellationToken) =>
#if NET45_OR_GREATER
                Task.Run(function, cancellationToken);
#else
                Run<Task>(function, cancellationToken).Unwrap();
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The function to execute asynchronously.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task<TResult> Run<TResult>(Func<Task<TResult>> function) =>
#if NET45_OR_GREATER
                Task.Run(function);
#else
                Run(function, CancellationToken.None);
#endif

            /// <summary>
            /// Creates a task that runs the specified function.
            /// </summary>
            /// <param name="function">The action to execute.</param>
            /// <param name="cancellationToken">The CancellationToken to use to cancel the task.</param>
            /// <returns>A task that represents the completion of the action.</returns>
            /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
            public static Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken) =>
#if NET45_OR_GREATER
                Task.Run(function, cancellationToken);
#else
                Run<Task<TResult>>(function, cancellationToken).Unwrap();
#endif

            /// <summary>
            /// Starts a Task that will complete after the specified due time.
            /// </summary>
            /// <param name="dueTime">The delay in milliseconds before the returned task completes.</param>
            /// <returns>The timed Task.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dueTime"/> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.</exception>
            public static Task Delay(int dueTime) =>
#if NET45_OR_GREATER
                Task.Delay(dueTime);
#else
                Delay(dueTime, CancellationToken.None);
#endif

            /// <summary>
            /// Starts a Task that will complete after the specified due time.
            /// </summary>
            /// <param name="dueTime">The delay before the returned task completes.</param>
            /// <returns>The timed Task.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dueTime"/> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.</exception>
            public static Task Delay(TimeSpan dueTime) =>
#if NET45_OR_GREATER
                Task.Delay(dueTime);
#else
                Delay(dueTime, CancellationToken.None);
#endif

            /// <summary>
            /// Starts a Task that will complete after the specified due time.
            /// </summary>
            /// <param name="dueTime">The delay before the returned task completes.</param>
            /// <param name="cancellationToken">A CancellationToken that may be used to cancel the task before the due time occurs.</param>
            /// <returns>The timed Task.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dueTime"/> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.</exception>
            public static Task Delay(TimeSpan dueTime, CancellationToken cancellationToken)
            {
#if NET45_OR_GREATER
                return Task.Delay(dueTime);
#else
                double num = dueTime.TotalMilliseconds;
                if (num < -1 || num > int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("dueTime", ArgumentOutOfRange_TimeoutNonNegativeOrMinusOne);
                }
                Contract.EndContractBlock();
                return Delay((int)num, cancellationToken);
#endif
            }

            /// <summary>
            /// Starts a Task that will complete after the specified due time.
            /// </summary>
            /// <param name="dueTime">The delay in milliseconds before the returned task completes.</param>
            /// <param name="cancellationToken">A CancellationToken that may be used to cancel the task before the due time occurs.</param>
            /// <returns>The timed Task.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dueTime"/> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.</exception>
            public static Task Delay(int dueTime, CancellationToken cancellationToken)
            {
#if NET45_OR_GREATER
                return Task.Delay(dueTime, cancellationToken);
#else
                if (dueTime < -1)
                {
                    throw new ArgumentOutOfRangeException("dueTime", ArgumentOutOfRange_TimeoutNonNegativeOrMinusOne);
                }
                Contract.EndContractBlock();
                if (cancellationToken.IsCancellationRequested)
                {
                    return new Task(() => { }, cancellationToken);
                }
                if (dueTime == 0)
                {
                    return s_preCompletedTask;
                }
                TaskCompletionSource<bool> tcs = new();
                CancellationTokenRegistration ctr = default;
                Timer? timer = null;
                timer = new Timer(_ =>
                {
                    ctr.Dispose();
                    timer!.Dispose();
                    tcs.TrySetResult(true);
                    TimerManager.Remove(timer);
                }, null, -1, -1);
                TimerManager.Add(timer);
                if (cancellationToken.CanBeCanceled)
                {
                    ctr = cancellationToken.Register(() =>
                    {
                        timer.Dispose();
                        tcs.TrySetCanceled();
                        TimerManager.Remove(timer);
                    });
                }
                timer.Change(dueTime, -1);
                return tcs.Task;
#endif
            }

            /// <summary>
            /// Creates a Task that will complete only when all of the provided collection of Tasks has completed.
            /// </summary>
            /// <param name="tasks">The Tasks to monitor for completion.</param>
            /// <returns>A Task that represents the completion of all of the provided tasks.</returns>
            /// <remarks>
            /// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
            /// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
            /// Task will also be canceled.
            /// </remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task WhenAll(params Task[] tasks) =>
#if NET45_OR_GREATER
                Task.WhenAll(tasks);
#else
                WhenAll((IEnumerable<Task>)tasks);
#endif

            /// <summary>
            /// Creates a Task that will complete only when all of the provided collection of Tasks has completed.
            /// </summary>
            /// <param name="tasks">The Tasks to monitor for completion.</param>
            /// <returns>A Task that represents the completion of all of the provided tasks.</returns>
            /// <remarks>
            /// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
            /// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
            /// Task will also be canceled.
            /// </remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks) =>
#if NET45_OR_GREATER
                Task.WhenAll(tasks);
#else
                WhenAll((IEnumerable<Task<TResult>>)tasks);
#endif

            /// <summary>
            /// Creates a Task that will complete only when all of the provided collection of Tasks has completed.
            /// </summary>
            /// <param name="tasks">The Tasks to monitor for completion.</param>
            /// <returns>A Task that represents the completion of all of the provided tasks.</returns>
            /// <remarks>
            /// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
            /// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
            /// Task will also be canceled.
            /// </remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task WhenAll(IEnumerable<Task> tasks) =>
#if NET45_OR_GREATER
                Task.WhenAll(tasks);
#else
                WhenAllCore<object?>(tasks, (completedTasks, tcs) => tcs.TrySetResult(null));
#endif

            /// <summary>
            /// Creates a Task that will complete only when all of the provided collection of Tasks has completed.
            /// </summary>
            /// <param name="tasks">The Tasks to monitor for completion.</param>
            /// <returns>A Task that represents the completion of all of the provided tasks.</returns>
            /// <remarks>
            /// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
            /// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
            /// Task will also be canceled.
            /// </remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks) =>
#if NET45_OR_GREATER
                Task.WhenAll(tasks);
#else
                WhenAllCore<TResult[]>(tasks.Cast<Task>(), (completedTasks, tcs) => tcs.TrySetResult([.. completedTasks.Select(t => ((Task<TResult>)t).Result)]));
#endif

            /// <summary>
            /// Creates a Task that will complete when any of the tasks in the provided collection completes.
            /// </summary>
            /// <param name="tasks">The Tasks to be monitored.</param>
            /// <returns>A Task that represents the completion of any of the provided Tasks. The completed Task is this Task's result.</returns>
            /// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<Task> WhenAny(params Task[] tasks) =>
#if NET45_OR_GREATER
                Task.WhenAny(tasks);
#else
                WhenAny((IEnumerable<Task>)tasks);
#endif

            /// <summary>
            /// Creates a Task that will complete when any of the tasks in the provided collection completes.
            /// </summary>
            /// <param name="tasks">The Tasks to be monitored.</param>
            /// <returns>A Task that represents the completion of any of the provided Tasks.  The completed Task is this Task's result.</returns>
            /// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<Task> WhenAny(IEnumerable<Task> tasks)
            {
#if NET45_OR_GREATER
                return Task.WhenAny(tasks);
#else
                ArgumentNullException.ThrowIfNull(tasks);
                Contract.EndContractBlock();
                TaskCompletionSource<Task> tcs = new();
                Task.Factory.ContinueWhenAny((tasks as Task[]) ?? [.. tasks], tcs.TrySetResult, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                return tcs.Task;
#endif
            }

            /// <summary>
            /// Creates a Task that will complete when any of the tasks in the provided collection completes.
            /// </summary>
            /// <param name="tasks">The Tasks to be monitored.</param>
            /// <returns>A Task that represents the completion of any of the provided Tasks. The completed Task is this Task's result.</returns>
            /// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks) =>
#if NET45_OR_GREATER
                Task.WhenAny(tasks);
#else
                WhenAny((IEnumerable<Task<TResult>>)tasks);
#endif

            /// <summary>
            /// Creates a Task that will complete when any of the tasks in the provided collection completes.
            /// </summary>
            /// <param name="tasks">The Tasks to be monitored.</param>
            /// <returns>A Task that represents the completion of any of the provided Tasks. The completed Task is this Task's result.</returns>
            /// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
            /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
            public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
            {
#if NET45_OR_GREATER
                return Task.WhenAny(tasks);
#else
                ArgumentNullException.ThrowIfNull(tasks);
                Contract.EndContractBlock();
                TaskCompletionSource<Task<TResult>> tcs = new();
                Task.Factory.ContinueWhenAny((tasks as Task<TResult>[]) ?? [.. tasks], tcs.TrySetResult, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                return tcs.Task;
#endif
            }

            /// <summary>
            /// Creates an already completed <see cref="Task{TResult}" /> from the specified result.
            /// </summary>
            /// <param name="result">The result from which to create the completed task.</param>
            /// <returns>The completed task.</returns>
            public static Task<TResult> FromResult<TResult>(TResult result)
            {
#if NET45_OR_GREATER
                return Task.FromResult(result);
#else
                TaskCompletionSource<TResult> taskCompletionSource = new(result);
                taskCompletionSource.TrySetResult(result);
                return taskCompletionSource.Task;
#endif
            }
        }

#if !NET45_OR_GREATER
        /// <summary>
        /// Creates a Task that will complete only when all of the provided collection of Tasks has completed.
        /// </summary>
        /// <param name="tasks">The Tasks to monitor for completion.</param>
        /// <param name="setResultAction">
        /// A callback invoked when all of the tasks complete successfully in the RanToCompletion state.
        /// This callback is responsible for storing the results into the TaskCompletionSource.
        /// </param>
        /// <returns>A Task that represents the completion of all of the provided tasks.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="tasks"/> argument contains a null reference.</exception>
        private static Task<TResult> WhenAllCore<TResult>(IEnumerable<Task> tasks, Action<Task[], TaskCompletionSource<TResult>> setResultAction)
        {
            ArgumentNullException.ThrowIfNull(tasks);
            Contract.EndContractBlock();
            Contract.Assert(setResultAction != null);
            TaskCompletionSource<TResult> tcs = new();
            Task[] array = (tasks as Task[]) ?? [.. tasks];
            if (array.Length == 0)
            {
                setResultAction!(array, tcs);
            }
            else
            {
                Task.Factory.ContinueWhenAll(array, completedTasks =>
                {
                    List<Exception?>? targetList = null;
                    bool flag = false;
                    foreach (Task task in completedTasks)
                    {
                        if (task.IsFaulted)
                        {
                            AddPotentiallyUnwrappedExceptions(ref targetList, task.Exception);
                        }
                        else if (task.IsCanceled)
                        {
                            flag = true;
                        }
                    }
                    if (targetList != null && targetList.Count > 0)
                    {
                        tcs.TrySetException(targetList);
                    }
                    else if (flag)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        setResultAction!(completedTasks, tcs);
                    }
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            return tcs.Task;
        }

        /// <summary>
        /// Adds the target exception to the list, initializing the list if it's null.
        /// </summary>
        /// <param name="targetList">The list to which to add the exception and initialize if the list is null.</param>
        /// <param name="exception">The exception to add, and unwrap if it's an aggregate.</param>
        private static void AddPotentiallyUnwrappedExceptions(ref List<Exception?>? targetList, Exception exception)
        {
            AggregateException? ex = exception as AggregateException;
            Contract.Assert(exception != null);
            Contract.Assert(ex == null || ex.InnerExceptions.Count > 0);
            targetList ??= [];
            targetList.Add(ex != null ? (ex.InnerExceptions.Count == 1) ? ex.InnerException : ex : exception);
        }
#endif
    }
}
