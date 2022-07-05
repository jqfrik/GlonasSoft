using GlonasSoft.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GlonasSoft.Bll.Services;

public class ProcessorRequests : BackgroundService
{
    private readonly ProcessorQueue _queue;
    private readonly IDbContextFactory<GlonasContext> _dbContext;

    public ProcessorRequests(ProcessorQueue queue,
        IDbContextFactory<GlonasContext> dbContext)
    {
        _queue = queue;
        _dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Process(stoppingToken);
    }

    private async Task Process(CancellationToken stoppingToken)
    {
        do
        {
            if (_queue.Tasks != null && _queue.Tasks.Any())
            {
                var context = await _dbContext.CreateDbContextAsync(stoppingToken);
                while (_queue.Tasks.Any())
                {
                    if (_queue.Tasks.TryDequeue(out var task))
                    {
                        task(context);
                    }
                }
            }

            await Task.Delay(1000);
        } while (!stoppingToken.IsCancellationRequested);
    }
}