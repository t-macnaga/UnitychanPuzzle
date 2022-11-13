using System.Collections;
using System.Collections.Generic;
using System;

namespace Lib.Commands
{
    public class CommandExecutionContext<TContext, TLogData>
       where TLogData : ILogData
    {
        public Queue<TLogData> LogQueue { get; set; } = new Queue<TLogData>();
        public event Action<TLogData> OnBeforeExecuteCommand;
        public event Action<TLogData> OnAfterExecuteCommand;
        public Dictionary<int, ICommand<TContext, TLogData>> Commands
                        = new Dictionary<int, ICommand<TContext, TLogData>>();

        public IEnumerator ExecuteAsync(TContext context)
        {
            while (true)
            {
                if (LogQueue.Count > 0)
                {
                    var log = LogQueue.Dequeue();
                    yield return ExecuteAsync(context, log);
                }
                else
                {
                    yield return null;
                }
            }
        }

        public IEnumerator ExecuteAsync(TContext context, TLogData log)
        {
            if (Commands.ContainsKey(log.Id))
            {
                OnBeforeExecuteCommand?.Invoke(log);
                yield return Commands[log.Id].ExecuteAsync(context, log);
                OnAfterExecuteCommand?.Invoke(log);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Commands not contains LogId:{log.Id} in dictionary");
            }
        }
    }
}