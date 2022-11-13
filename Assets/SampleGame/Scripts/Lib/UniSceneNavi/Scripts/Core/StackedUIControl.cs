using System.Collections.Generic;
using UnityEngine;

public class StackedUIControl : MonoBehaviour
{
    [SerializeField] Transform parent = default;

    Stack<GameObject> goStack = new Stack<GameObject>();

    public bool CanGoBack()
    {
        return goStack.Count > 0;
    }

    /// <summary>
    /// GameObjectをスタックにプッシュします。
    /// </summary>
    /// <param name="go">スタックするオブジェクト</param>
    /// <param name="activeCurrent">スタックでトップにいるGameObjectをアクティブのままにしておくかどうか。
    /// trueならアクティブのままにしておきます</param>
    public void Push(GameObject go, bool activeCurrent = true)
    {
        if (goStack.Count > 0)
        {
            goStack.Peek().SetActive(activeCurrent);
        }
        go.transform.SetParent(parent, false);
        goStack.Push(go);
    }

    /// <summary>
    /// GameObjectをスタックからポップします。
    /// </summary>
    public GameObject Pop()
    {
        if (goStack.Count <= 0) { return null; }

        // 現在トップにいるものを破棄
        var go = goStack.Pop();
        Destroy(go);

        // スタックに残っていれば現在トップのものをアクティブ
        if (goStack.Count > 0)
        {
            goStack.Peek().SetActive(true);
            return goStack.Peek();
        }
        return null;
    }
}
