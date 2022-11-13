using System;
using System.Collections;
using System.Threading;
using UniRx;

public interface IState
{
    void Enter();
    IEnumerator ExecuteAsync(IStateMachine stateMachine);
    void Exit();
}

public interface IStateMachine
{
    IState Current { get; }
    void ChangeState(IState state);
}

public class State : IState
{
    virtual public void Enter()
    {
    }

    virtual public IEnumerator ExecuteAsync(IStateMachine stateMachine)
    {
        yield break;
    }

    virtual public void Exit()
    {
    }
}

public class StateMachine : IStateMachine, IDisposable
{
    public IState Current { get; private set; }

    IDisposable disposable;
    IEnumerator enumerator;

    public void Dispose()
    {
        disposable.Dispose();
    }

    public void ChangeState(IState state)
    {
        disposable?.Dispose();
        Current?.Exit();
        Current = state;
        Current.Enter();
        enumerator = Current.ExecuteAsync(this);
        disposable = Observable.FromCoroutine(c => ExecuteAsync(c)).Subscribe();
    }

    IEnumerator ExecuteAsync(CancellationToken cancellationToken)
    {
        var hasNext = default(bool);
        do
        {
            try
            {
                hasNext = enumerator.MoveNext();
            }
            catch (Exception ex)
            {
                try
                {
                    //                    observer.OnError(ex);
                }
                finally
                {
                    var d = enumerator as IDisposable;
                    if (d != null)
                    {
                        d.Dispose();
                    }
                }
                yield break;
            }
            if (hasNext)
            {
                yield return enumerator.Current; // yield inner YieldInstruction
            }
        } while (hasNext && !cancellationToken.IsCancellationRequested);

        UnityEngine.Debug.Log("End Exec");
    }
}