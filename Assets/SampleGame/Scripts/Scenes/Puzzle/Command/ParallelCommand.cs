using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelCommand : IPuzzleCommand
{
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        var coroutineList = new List<Coroutine>();
        foreach (var child in log.children)
        {
            var executor = context.Commands.ExecutionContext.ExecuteAsync(context, child);
            var coroutine = context.Director.StartCoroutine(executor);
            coroutineList.Add(coroutine);
        }
        foreach (var coroutine in coroutineList)
        {
            yield return coroutine;
        }
    }
}
