using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneParam<T>
{
    public string sceneName { get; private set; }
    public T Argument { get; private set; }
    public SceneBase SceneBase { get; set; }
    public bool IsStack { get; private set; }
    public LoadSceneMode LoadSceneMode { get; private set; }
    public SceneParam(
        string sceneName,
        T argument,
        bool isStack,
        LoadSceneMode loadSceneMode)
    {
        this.sceneName = sceneName;
        this.Argument = argument;
        this.IsStack = isStack;
        this.LoadSceneMode = loadSceneMode;
    }
}

public class SceneParam : SceneParam<object>
{
    public SceneParam(
        string sceneName,
        object argument,
        bool isStack,
        LoadSceneMode loadSceneMode) : base(sceneName, argument, isStack, loadSceneMode)
    {
    }

    public SceneParam<T> ParseTo<T>()
    {
        return new SceneParam<T>(
            sceneName,
            JsonUtility.FromJson<T>(Argument as string),
            IsStack,
            LoadSceneMode);
    }
}
