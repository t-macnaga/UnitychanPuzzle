using System.Collections;

namespace Lib.Commands
{
    public interface ILogData
    {
        int Id { get; }
    }

    public interface ICommand<TContext, in TLogData>
        where TLogData : ILogData
    {
        IEnumerator ExecuteAsync(TContext context, TLogData log);
    }
}