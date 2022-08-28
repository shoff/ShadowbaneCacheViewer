namespace Shadowbane.Viewer;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;

public interface IAsyncCommand : ICommand
{
    bool CanExecute();
    Task ExecuteAsync();
    IEnumerable<Task> RunningTasks { get; }
}

public interface IAsyncCommand<in T> : ICommand
{
    IEnumerable<Task> RunningTasks { get; }
    bool CanExecute(T? parameter);
    Task ExecuteAsync(T? parameter);
}

public abstract class AsyncCommand : IAsyncCommand
{
    private readonly ObservableCollection<Task> runningTasks;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
    /// </summary>
    protected AsyncCommand()
    {
        runningTasks = new ObservableCollection<Task>();
        runningTasks.CollectionChanged += OnRunningTasksChanged;
    }

    /// <inheritdoc />
    public IEnumerable<Task> RunningTasks
    {
        get => runningTasks;
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <inheritdoc />
    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute();
    }

    /// <inheritdoc />
    public abstract bool CanExecute();

    /// <inheritdoc />
    async void ICommand.Execute(object? parameter)
    {
        Task runningTask = ExecuteAsync();
        runningTasks.Add(runningTask);

        try
        {
            await runningTask;
        }
        finally
        {
            runningTasks.Remove(runningTask);
        }
    }

    /// <inheritdoc />
    public abstract Task ExecuteAsync();

    private void OnRunningTasksChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
    }
}

public abstract class AsyncCommand<T> : IAsyncCommand<T>
{
    private readonly ObservableCollection<Task> runningTasks;

    protected AsyncCommand()
    {
        runningTasks = new ObservableCollection<Task>();
        runningTasks.CollectionChanged += OnRunningTasksChanged;
    }
    
    public IEnumerable<Task> RunningTasks
    {
        get => runningTasks;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute((T)parameter!);
    }
    
    public abstract bool CanExecute(T? parameter);
    
    async void ICommand.Execute(object? parameter)
    {
        Task runningTask = ExecuteAsync((T)parameter!);

        runningTasks.Add(runningTask);

        try
        {
            await runningTask;
        }
        finally
        {
            runningTasks.Remove(runningTask);
        }
    }

    /// <inheritdoc />
    public abstract Task ExecuteAsync(T? parameter);

    private void OnRunningTasksChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
