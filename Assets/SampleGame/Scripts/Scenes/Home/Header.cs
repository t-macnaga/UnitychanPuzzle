using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Header : MonoBehaviour
{
    [SerializeField] Button backButton = default;

    void Awake()
    {
        backButton.OnClickAsObservable().Subscribe(_ =>
        {
            NavigationService.Broker.Publish<BackButtonEvent>(new BackButtonEvent());
        }).AddTo(this);
    }
}
