using System;
using System.Collections;
using System.Threading;
using UniRx;

public interface IState<TContext>
{
    void Enter(TContext context);
    // IEnumerator ExecuteAsync(TContext context);
    void Update(TContext context);
    void Exit(TContext context);
}

// public class State : IState<TContext>
// {
//     virtual public void Enter()
//     {
//     }

//     virtual public IEnumerator ExecuteAsync(TContext context)
//     {
//         yield break;
//     }

//     virtual public void Exit()
//     {
//     }
// }

public class StateMachine<TContext> : IDisposable
{
    public IState<TContext> Current { get; private set; }
    public TContext Context { get; set; }

    IDisposable disposable;
    IEnumerator enumerator;

    public void Dispose()
    {
        disposable.Dispose();
    }

    public void ChangeState(IState<TContext> state)
    {
        disposable?.Dispose();
        Current?.Exit(Context);
        Current = state;
        Current.Enter(Context);
        // enumerator = Current.ExecuteAsync(Context);
        // disposable = Observable.FromCoroutine(c => ExecuteAsync(c)).Subscribe();
    }

    public void Update()
    {
        Current?.Update(Context);
    }

    // IEnumerator ExecuteAsync(CancellationToken cancellationToken)
    // {
    //     var hasNext = default(bool);
    //     do
    //     {
    //         try
    //         {
    //             hasNext = enumerator.MoveNext();
    //         }
    //         catch (Exception ex)
    //         {
    //             try
    //             {
    //                 //                    observer.OnError(ex);
    //             }
    //             finally
    //             {
    //                 var d = enumerator as IDisposable;
    //                 if (d != null)
    //                 {
    //                     d.Dispose();
    //                 }
    //             }
    //             yield break;
    //         }
    //         if (hasNext)
    //         {
    //             yield return enumerator.Current; // yield inner YieldInstruction
    //         }
    //     } while (hasNext && !cancellationToken.IsCancellationRequested);

    //     UnityEngine.Debug.Log("End Exec");
    // }
}