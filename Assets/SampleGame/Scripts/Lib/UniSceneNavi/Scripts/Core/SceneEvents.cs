using UnityEngine.SceneManagement;

public struct AfterLoadSceneEvent
{
    public LoadSceneMode LoadSceneMode { get; private set; }
    public AfterLoadSceneEvent(LoadSceneMode loadSceneMode)
    {
        LoadSceneMode = loadSceneMode;
    }
}

public struct AsyncBeginLoadSceneEvent
{
    public LoadSceneMode LoadSceneMode { get; private set; }
    public AsyncBeginLoadSceneEvent(LoadSceneMode loadSceneMode)
    {
        LoadSceneMode = loadSceneMode;
    }
}

public struct AsyncAfterLoadSceneEvent
{
    public LoadSceneMode LoadSceneMode { get; private set; }
    public AsyncAfterLoadSceneEvent(LoadSceneMode loadSceneMode)
    {
        LoadSceneMode = loadSceneMode;
    }
}

/// <summary>
/// バックキーや戻るボタンが押されたときに発火するイベント
/// </summary>
public struct BackButtonEvent
{
}

/// <summary>
/// バックキーや戻るボタンが押されたがこれ以上戻れないときに発火するイベント
/// </summary>
public struct NoMoreBackEvent
{
}