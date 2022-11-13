using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] StackedUIControl stackedUIControl = default;

    static public PopupManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void Push(GameObject go)
    {
        stackedUIControl.Push(go, false);
    }

    public GameObject Pop()
    {
        return stackedUIControl.Pop();
    }

    public bool CanGoBack()
    {
        return stackedUIControl.CanGoBack();
    }
}
