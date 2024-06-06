using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
/// </summary>
public abstract class BackgroundWorkerBase : IBackgroundWorker
{
    //TODO: Add UOW, Localization and other useful properties..?

    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    public IServiceProvider ServiceProvider { get; set; } = default!;

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    protected CancellationTokenSource StoppingTokenSource { get; private set; }
	
    protected CancellationToken StoppingToken { get; private set; }

    public BackgroundWorkerBase()
    {
        ResetStoppingCancellationTokenSource();
    }

    private void ResetStoppingCancellationTokenSource()
    {
        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Started background worker: " + ToString());
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Stopped background worker: " + ToString());
        StoppingTokenSource.Cancel();
        StoppingTokenSource.Dispose();

        ResetStoppingCancellationTokenSource();
        
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return GetType().FullName!;
    }
}
