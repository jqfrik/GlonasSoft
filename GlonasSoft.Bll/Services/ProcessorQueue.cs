using System.Collections.Concurrent;
using GlonasSoft.Dal;

namespace GlonasSoft.Bll.Services;

public class ProcessorQueue
{
    public ConcurrentQueue<Func<GlonasContext, Task>> Tasks { get; set; } = new();
}